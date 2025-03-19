using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.Services.FacultyService;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddFaculty(FacultyDTO facultyDTO)
        {
            var result = await _facultyService.AddFacultyAsync(facultyDTO);
            if (result.Success) return Ok(ApiResponse<FacultyDTO>.Success(
                    data: result.Data,
                    message: "Faculty added successfully"
                ));

            // Failed
            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to add faculty"
                    }
                ));
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllFaculties()
        {
            var result = await _facultyService.GetAllFacultiesAsync();
            if (result.Success) return Ok(ApiResponse<IEnumerable<FacultyDTO>>.Success(
                    data: result.Data,
                    message: "Faculties retrieved successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to retrieve faculties"
                    }
                ));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaculty(string id, FacultyDTO facultyDTO)
        {
            var result = await _facultyService.UpdateFacultyAsync(id, facultyDTO);
            if (result.Success) return Ok(ApiResponse<FacultyDTO>.Success(
                    data: result.Data,
                    message: "Faculty name changed successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to change faculty name"
                    }
                ));
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            var result = await _facultyService.DeleteFacultyAsync(id);
            if (result.Success) return Ok(ApiResponse<FacultyDTO>.Success(
                    data: result.Data,
                    message: "Faculty deleted successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to delete faculty"
                    }
                ));
        }

        [HttpDelete("name/{name}")]
        public async Task<IActionResult> DeleteFacultyByName(string name)
        {
            var result = await _facultyService.DeleteFacultyAsync(name);
            if (result.Success) return Ok(ApiResponse<FacultyDTO>.Success(
                    data: result.Data,
                    message: "Faculty deleted successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to delete faculty"
                    }
                ));
        }
    }
}
