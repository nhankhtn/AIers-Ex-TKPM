using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ApiResponse<AddClassDTO>>> AddClass(AddClassDTO addClassDTO)
        {
            var result = await _classService.AddClassAsync(addClassDTO);
            if (!result.Success)
            { 
                return BadRequest(new ApiResponse<AddClassDTO>
                {
                    Error = new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                });  
            }
            return Ok(new ApiResponse<AddClassDTO>
            {
                Data = result.Data
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> GetClass(int id)
        {
            var result = await _classService.GetClassAsync(id);
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
            return Ok(new ApiResponse<GetClassDTO>
            {
                Data = result.Data
            });
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetClassDTO>>>> GetClasses(string? course = null)
        {
            var result = await _classService.GetClassesAsync(course);
            if (!result.Success)
            {
                return BadRequest(new ApiResponse<IEnumerable<GetClassDTO>>
                {
                    Error = new ApiError
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                });
            }
            return Ok(new ApiResponse<IEnumerable<GetClassDTO>>
            {
                Data = result.Data
            });
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> DeleteClass(int id)
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
            return Ok(new ApiResponse<GetClassDTO>
            {
                Data = result.Data
            });
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<GetClassDTO>>> UpdateClass(int id, UpdateClassDTO updateClassDTO)
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
            return Ok(new ApiResponse<GetClassDTO>
            {
                Data = result.Data
            });
        }
    }
}
