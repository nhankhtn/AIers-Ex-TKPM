using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.BLL.Services.ClassStudentService;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using System.Runtime.CompilerServices;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : Controller
    {
        private readonly IClassStudentService _classStudentService;
        public ScoreController(IClassStudentService classStudentService)
        {
            _classStudentService = classStudentService;
        }

        [HttpPost]
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

        [HttpGet]
        public async Task<IActionResult> GetAllScores (string classId)
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
