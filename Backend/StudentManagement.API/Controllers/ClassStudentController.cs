using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Score;
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

        [HttpGet]
        public async Task<ActionResult<GetClassStudentsDTO>> GetClassStudents(string? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            var result = await _classStudentService.GetClassStudentsAsync(classId, studentId, page, limit);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(new ApiResponse<string>(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return NotFound(new ApiResponse<string>(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }

        [HttpPost("{studentId}")]
        public async Task<ActionResult<ApiResponse<GetClassStudentDTO>>> AddClassStudent(string studentId, IEnumerable<string> classIds)
        {
            var result = await _classStudentService.AddStudentAsync(studentId, classIds);
            if (result.Success)
            {
                return Ok(ApiResponse<IEnumerable<GetClassStudentDTO>>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<IEnumerable<GetClassStudentDTO>>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
        }

        [HttpGet("registerable-classes")]        
        public async Task<ActionResult<ApiResponse<IEnumerable<GetClassDTO>>>> GetRegisterableClasses(string studentId)
        {
            var result = await _classStudentService.GetRegisterableClassesAsync(studentId);
            if (result.Success)
            {
                return Ok(ApiResponse<IEnumerable<GetClassDTO>>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<IEnumerable<GetClassDTO>>.BadRequest(
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

        //[HttpPut]
        //public async Task<ActionResult<ApiResponse<IEnumerable<GetScoreDTO>>>> UpdateClassStudent(string classId, IEnumerable<UpdateScoreDTO> updateScoreDTO)
        //{
        //    var result = await _classStudentService.UpdateScoresAsync(classId, updateScoreDTO);
        //    if (result.Success)
        //    {
        //        return Ok(ApiResponse<IEnumerable<GetScoreDTO>>.Success(
        //            data: result.Data
        //        ));
        //    }
        //    return BadRequest(ApiResponse<IEnumerable<GetScoreDTO>>.BadRequest(
        //            error: new ApiError()
        //            {
        //                Code = result.ErrorCode,
        //                Message = result.ErrorMessage
        //            }
        //        ));
        //}
    }
}

