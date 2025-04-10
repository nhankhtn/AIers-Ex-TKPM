using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.ClassStudentService;
using System.Runtime.CompilerServices;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassStudentController : ControllerBase
    {
        private readonly IClassStudentService _classStudentService;

        public ClassStudentController(IClassStudentService classStudentService)
        {
            _classStudentService = classStudentService;
        }


        //[HttpGet]
        //public async Task<ActionResult<ApiResponse<GetStudentsDTO>>> GetClassStudent(int classId, string studentId)
        //{
        //    var result = await _classStudentService.GetClassStudentsAsync(classId, studentId);
        //    if (result.Success)
        //    {
        //        if (result.Data is null) return NotFound(new ApiResponse<string>(
        //            error: new ApiError()
        //            {
        //                Code = result.ErrorCode,
        //                Message = result.ErrorMessage
        //            }
        //        ));
        //        return Ok(result.Data);
        //    }
        //    return NotFound(new ApiResponse<string>(
        //        error: new ApiError()
        //        {
        //            Code = result.ErrorCode,
        //            Message = result.ErrorMessage
        //        }
        //    ));
        //}

        [HttpGet]
        public async Task<ActionResult<ApiResponse<GetClassStudentsDTO>>> GetClassStudents(int? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            var result = await _classStudentService.GetClassStudentsAsync(classId, studentId, page, limit);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(new ApiResponse<GetClassStudentsDTO>(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return NotFound(new ApiResponse<GetClassStudentsDTO>(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GetClassStudentDTO>>> AddClassStudent(AddStudentToClassDTO addStudentToClassDTO)
        {
            var result = await _classStudentService.AddStudentAsync(addStudentToClassDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<GetClassStudentDTO>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<GetClassStudentDTO>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }


        [HttpDelete]
        public async Task<ActionResult<ApiResponse<RegisterCancelationDTO>>> RegisterCancelation(RegisterCancelationDTO registerCancelationDTO)
        {
            var result = await _classStudentService.RegisterCancelationAsync(registerCancelationDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<RegisterCancelationDTO>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<RegisterCancelationDTO>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<GetClassStudentDTO>>> UpdateClassStudent(int classId, string studentId, UpdateClassStudentDTO updateClassStudentDTO)
        {
            var result = await _classStudentService.UpdateClassStudentAsync(classId, studentId, updateClassStudentDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<GetClassStudentDTO>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<GetClassStudentDTO>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }
    }
}

