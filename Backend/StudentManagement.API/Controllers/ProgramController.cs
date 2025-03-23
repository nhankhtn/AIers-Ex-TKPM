using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Program;
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
                    message: res.Message
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = res.ErrorMessage
                    }
                ));
            
        }

        [HttpPost()]
        public async Task<IActionResult> AddProgram(ProgramDTO programDTO)
        {
            var res = await _programService.AddProgramAsync(programDTO);
            if (res.Success) return Ok(ApiResponse<ProgramDTO>.Success(
                    data: res.Data,
                    message: res.ErrorMessage
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = res.ErrorMessage
                    }
                ));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(string id, ProgramDTO programDTO)
        {
            var res = await _programService.UpdateProgramAsync(id, programDTO);
            if (res.Success) return Ok(ApiResponse<ProgramDTO>.Success(
                    data: res.Data,
                    message: res.Message
                ));

            return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = res.ErrorCode,
                        Message = res.ErrorMessage
                    }
                ));
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteProgram(string id)
        {
            var result = await _programService.DeleteProgramAsync(id);
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

        [HttpDelete("name/{name}")]
        public async Task<IActionResult> DeleteProgramByName(string name)
        {
            var result = await _programService.DeleteProgramAsync(name);
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

    }
}
