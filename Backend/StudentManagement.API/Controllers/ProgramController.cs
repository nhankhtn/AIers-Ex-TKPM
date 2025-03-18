using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.Services.ProgramService;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : Controller
    {

        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllProgram()
        {
            var res = await _programService.GetAllProgramAsync();
            if (res.Success) return Ok(ApiResponse<IEnumerable<ProgramDTO>>.Success(
                    data: res.Data,
                    message: "Programs retrieved successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = "Failed to retrieve programs"
                    }
                ));
            
        }

        [HttpPost()]
        public async Task<IActionResult> AddProgram(ProgramDTO programDTO)
        {
            var res = await _programService.AddProgramAsync(programDTO);
            if (res.Success) return Ok(ApiResponse<ProgramDTO>.Success(
                    data: res.Data,
                    message: "Program added successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = "Failed to add program"
                    }
                ));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(string id, ProgramDTO programDTO)
        {
            var res = await _programService.UpdateProgramAsync(id, programDTO);
            if (res.Success) return Ok(ApiResponse<ProgramDTO>.Success(
                    data: res.Data,
                    message: "Program name changed successfully"
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = "Failed to change program name"
                    }
                ));
        }

    }
}
