using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.StudentStatus;
using StudentManagement.BLL.Services.StudentStatusService;

namespace StudentManagement.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StudentStatusController : Controller
    {

        private readonly IStudentStatusService _studentStatusService;

        public StudentStatusController(IStudentStatusService studentStatusService)
        {
            _studentStatusService = studentStatusService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllStudentStatus()
        {
            var result = await _studentStatusService.GetAllStudentStatusAsync();
            if (result.Success) return Ok(ApiResponse<IEnumerable<StudentStatusDTO>>.Success(
                    data: result.Data,
                    message: "Student status retrieved successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to retrieve student status"
                    }
                ));
        }

        [HttpPost()]
        public async Task<IActionResult> AddStudentStatus(StudentStatusDTO studentStatusDTO)
        {
            var result = await _studentStatusService.AddStudentStatusAsync(studentStatusDTO);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
                    data: result.Data,
                    message: "Student status added successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to add student status"
                    }
                ));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentStatus(string id, StudentStatusDTO studentStatusDTO)
        {
            var result = await _studentStatusService.UpdateStudentStatusAsync(id, studentStatusDTO);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
                    data: result.Data,
                    message: "Student status name changed successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = "Failed to change student status name"
                    }
                ));
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteFaculty(string id)
        {
            var result = await _studentStatusService.DeleteStudentStatusAsync(id);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
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
        public async Task<IActionResult> DeleteProgramByName(string name)
        {
            var result = await _studentStatusService.DeleteStudentStatusAsync(name);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
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
