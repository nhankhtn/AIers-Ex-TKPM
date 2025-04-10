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

        [HttpDelete("{courseId:int}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var result = await _courseService.DeleteCourseAsync(courseId);
            if (result.Success)
            {
                //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] khi gặp này thì kiểu int có default là 0 khôn có giá trị null nên báo lỗi
                return Ok(ApiResponse<string>.Success(data: result.Data.ToString(), message: result.Message));
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }
            ));
        }

        [HttpPut("{courseId:int}")]
        public async Task<IActionResult> UpdateCourseById(int courseId, UpdateCourseDTO course)
        {
            var result = await _courseService.UpdateCourseByIdAsync(courseId, course);
            if (result.Success)
            {
                return Ok(ApiResponse<UpdateCourseDTO>.Success(data: result.Data, message: result.Message));
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = await _courseService.GetAllCourseAsync();
            if (result.Success)
            {
                return Ok(ApiResponse<List<GetCourseDTO>>.Success(data: result.Data, message: result.Message));
            }
            return BadRequest(ApiResponse<List<GetCourseDTO>>.BadRequest(
                error: new ApiError
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
                }));
        }

        [HttpGet("{courseId:int}")]
        public async Task<IActionResult> GetStudentById(int courseId)
        {
            var result = await _courseService.GetAllCourseByIdAsync(courseId);
            if (result.Success)
            {
                return Ok(ApiResponse<GetCourseDTO>.Success(data: result.Data, message: result.Message));
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
