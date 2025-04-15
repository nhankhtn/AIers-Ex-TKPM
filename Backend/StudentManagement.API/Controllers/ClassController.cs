using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.BLL.Services.ClassService;
using StudentManagement.BLL.Services.ClassStudentService;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IClassStudentService _classStudentService;
        public ClassController(IClassService classService, IClassStudentService classStudentService)
        {
            _classService = classService;
            _classStudentService = classStudentService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> AddClass(AddClassDTO addClassDTO)
        {
            var result = await _classService.AddClassAsync(addClassDTO);
            if (!result.Success)
            { 
                return BadRequest(new ApiResponse<GetClassDTO>
                {
                    Error = new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                });  
            }
            return Ok(result.Data);
        }


        [HttpGet]
        public async Task<ActionResult<GetClassesDTO>> GetClasses(string? classId = null, int? semester = null, int? year = null, int? page = null, int? limit = null)
        {
            var result = await _classService.GetClassesAsync(classId, semester, year, page, limit);
            if (!result.Success)
            {
                return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
            }
            return Ok(result.Data);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> DeleteClass(string id)
        {
            var result = await _classService.DeleteClassAsync(id);
            if (!result.Success)
            {
                return BadRequest(new ApiResponse<GetClassDTO>
                {
                    Error = new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                });
            }
            return Ok(result.Data);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> UpdateClass(string id, UpdateClassDTO updateClassDTO)
        {
            var result = await _classService.UpdateClassAsync(id, updateClassDTO);
            if (!result.Success)
            {
                return BadRequest(new ApiResponse<GetClassDTO>
                {
                    Error = new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                });
            }
            return Ok(result.Data);
        }


        [HttpPost("scores")]
        public async Task<IActionResult> UpdateScore(string classId, IEnumerable<UpdateScoreDTO> updateScoresDTO)
        {
            var result = await _classStudentService.UpdateScoresAsync(classId, updateScoresDTO);
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

        [HttpGet("scores")]
        public async Task<IActionResult> GetAllScores(string classId)
        {
            var result = await _classStudentService.GetScoresAsync(classId);
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
