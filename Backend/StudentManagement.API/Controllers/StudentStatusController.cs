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

        [HttpPost()]
        public async Task<IActionResult> AddStudentStatus(StudentStatusDTO studentStatusDTO)
        {
            var result = await _studentStatusService.AddStudentStatusAsync(studentStatusDTO);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
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
        public async Task<IActionResult> UpdateStudentStatus(string id, StudentStatusDTO studentStatusDTO)
        {
            var result = await _studentStatusService.UpdateStudentStatusAsync(id, studentStatusDTO);
            if (result.Success) return Ok(ApiResponse<StudentStatusDTO>.Success(
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
            var result = await _studentStatusService.DeleteStudentStatusAsync(id);
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
        //public async Task<IActionResult> DeleteProgramByName(string name)
        //{
        //    var result = await _studentStatusService.DeleteStudentStatusAsync(name);
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
