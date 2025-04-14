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
                    message: result.Message
                ));

            // Failed
            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllFaculties()
        {
            var result = await _facultyService.GetAllFacultiesAsync();
            if (result.Success) return Ok(ApiResponse<IEnumerable<FacultyDTO>>.Success(
                    data: result.Data,
                    message: result.Message
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaculty(string id, FacultyDTO facultyDTO)
        {
            var result = await _facultyService.UpdateFacultyAsync(id, facultyDTO);
            if (result.Success) return Ok(ApiResponse<FacultyDTO>.Success(
                    data: result.Data,
                    message: result.Message
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            var result = await _facultyService.DeleteFacultyAsync(id);
            if (result.Success) return Ok(ApiResponse<string>.Success(
                    data: result.Data,
                    message: result.Message
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }

        //[HttpDelete("name/{name}")]
        //public async Task<IActionResult> DeleteFacultyByName(string name)
        //{
        //    var result = await _facultyService.DeleteFacultyAsync(name);
        //    if (result.Success) return Ok(ApiResponse<string>.Success(
        //            data: result.Data,
        //            message: result.Message
        //        ));

        //    return BadRequest(ApiResponse<string>.BadRequest(
        //            error: new ApiError()
        //            {
        //                Code = result.ErrorCode,
        //                Message = result.ErrorMessage
        //            }
        //        ));
        //}
    }
}
