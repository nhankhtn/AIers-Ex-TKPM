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

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseDTO courseDTO)
        {
            var result = await _courseService.AddCourseAsync(courseDTO);
            if (result.Success)
            {
                return Created(string.Empty, result.Data);
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            var result = await _courseService.DeleteCourseAsync(courseId);
            if (result.Success)
            {
                //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] khi gặp này thì kiểu int có default là 0 khôn có giá trị null nên báo lỗi
                return Ok(ApiResponse<string>.Success(data: result.Data, message: result.Message));
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourseById(string courseId, UpdateCourseDTO course)
        {
            var result = await _courseService.UpdateCourseByIdAsync(courseId, course);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllStudent(int page, int limit, Guid? facultyId, string? courseId, bool isDeleted = false)
        {
            var result = await _courseService.GetAllCourseAsync(page, limit, facultyId, courseId, isDeleted);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<GetAllCoursesDTO>.BadRequest(
                error: new ApiError
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }));
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetStudentById(string courseId)
        {
            var result = await _courseService.GetAllCourseByIdAsync(courseId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(ApiResponse<GetCourseDTO>.BadRequest(
                error: new ApiError
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }));
        }
    }
}
