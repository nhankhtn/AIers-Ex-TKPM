using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs;
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
        public async Task<IActionResult> ChangeStudentStatusName(int id, StudentStatusDTO studentStatusDTO)
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
    }
}
