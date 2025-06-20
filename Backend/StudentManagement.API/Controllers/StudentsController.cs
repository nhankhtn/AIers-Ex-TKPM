using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.ClassService;
using StudentManagement.BLL.Services.ClassStudentService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IClassStudentService _classStudentService;
        public StudentsController(IStudentService studentService, IClassService classService, IClassStudentService classStudentService)
        {
            _studentService = studentService;
            _classService = classService;
            _classStudentService = classStudentService;
        }

        [HttpGet]
        public async Task<ActionResult<GetStudentsDTO>> GetAllStudents(int? page, int? limit, string? faculty, string? program, string? status, string? key)
        {
            var result = await _studentService.GetAllStudentsAsync(page, limit, faculty, program, status, key);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(ApiResponse<string>.NotFound(
                    error: new ApiError() {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentDTO>>>> AddStudents(IEnumerable<StudentDTO> studentDTOs)
        {
            var result = await _studentService.AddListStudentAsync(studentDTOs);

            if (result.Success)
            {
                return Created(string.Empty, ApiResponse<IEnumerable<StudentDTO>>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<IEnumerable<StudentDTO>>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    },
                    errors: result.Errors?.Select(e => new ApiError()
                    {
                        Index = e.index,
                        Code = e.errorCode,
                        Message = e.errorMessage
                    }).ToList()
            ));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDTO>>> UpdateStudent(string id, StudentDTO updateStudentDTO)
        {
            var result = await _studentService.UpdateStudentAsync(id, updateStudentDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<StudentDTO>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<StudentDTO>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAsync(string id)
        {
            var res = await _studentService.DeleteStudentByIdAsync(id);
            if (res.Success)
            {
                return Ok(ApiResponse<string>.Success(
                    data: res.Data
                ));
            }
            else
            {
                return NotFound(ApiResponse<string>.NotFound(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = res.ErrorMessage
                    }
                ));
            }
        }

        [HttpGet("class-students")]
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

        [HttpPost("register/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetClassStudentDTO>>), 207)]
        public async Task<ActionResult<ApiResponse<GetClassStudentDTO>>> AddClassStudent(string studentId, IEnumerable<string> classIds)
        {
            var result = await _classStudentService.AddStudentAsync(studentId, classIds);
            if (result.Success)
            {
                return StatusCode(207, ApiResponse<IEnumerable<GetClassStudentDTO>>.MultiStatus(
                    data: result.Data,
                    message: result.Message,
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    },
                    errors: result.Errors?.Select(e => new ApiError()
                    {
                        Index = e.index,
                        Code = e.errorCode,
                        Message = e.errorMessage
                    }).ToList()
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


        [HttpDelete("register")]
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

        [HttpGet("register-cancelation")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RegisterCancelationDTO>>>> GetRegisterCancelationHistory(int? page, int? limit, string? key)
        {
            var result = await _classStudentService.GetRegisterCancelationHistoryAsync(page, limit, key);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(ApiResponse<string>.NotFound(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }


        [HttpGet("transcript")]
        public async Task<ActionResult<ApiResponse<StudentTranscriptDTO>>> StudentTranscipt(string studentId)
        {
            var result = await _classStudentService.GetStudentTranscriptAsync(studentId);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(ApiResponse<string>.NotFound(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }
    }
}