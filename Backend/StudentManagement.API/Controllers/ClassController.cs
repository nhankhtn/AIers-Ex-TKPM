using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.Services.ClassService;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
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


        [HttpDelete]
        [Route("{id}")]
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


        [HttpPut]
        [Route("{id}")]
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
    }
}
