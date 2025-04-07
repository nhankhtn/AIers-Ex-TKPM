using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.Services.CourseService;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost()]
        public async Task<IActionResult> AddCourse(AddCourseDTO courseDTO)
        {
            var result = await _courseService.AddCourseAsync(courseDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<AddCourseDTO>.Success(data : result.Data, message: result.Message));
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
