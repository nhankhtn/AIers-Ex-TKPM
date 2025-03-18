using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Utils;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.StudentService;
using System.Runtime.CompilerServices;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllStudents(int page, int limit, string? key)
        {
            var result = await _studentService.GetAllStudentsAsync(page, limit, key);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(ApiResponse<IEnumerable<StudentDTO>>.NotFound(
                    error: new ApiError() { 
                        Code = "STUDENT_NOT_FOUND",
                        Message = "No student found"
                    }
                ));
                return Ok(new
                {
                    title = "Success",
                    status = 200,
                    data = result.Data.Students,
                    total = result.Data.Total
                });
            }
            return NotFound(new
            {
                title = "Not Found",
                status = 404,
                error = new
                {
                    code = result.ErrorCode,
                }
            });
        }



        //[HttpPost]
        //public async Task<IActionResult> AddStudent(AddListStudentDTO studentDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var firstError = ModelState.Values
        //        .SelectMany(x => x.Errors)
        //        .Select(e => e.ErrorMessage)
        //        .FirstOrDefault();

        //        return BadRequest(ApiResponse<string>.BadRequest(
        //            error: new ApiError()
        //            {
        //                Code = firstError
        //            }
        //        ));
        //    }
        //    var result = await _studentService.AddStudentAsync(studentDTO);

        //    if (result.Success)
        //    {
        //        return Ok(ApiResponse<StudentDTO>.Success(
        //            data: result.Data
        //        ));
        //    }
        //    return BadRequest(ApiResponse<string>.BadRequest(
        //            error: new ApiError()
        //            {
        //                Code = result.ErrorCode
        //            }
        //    ));
        //}




        [HttpPut("{id}")]
        public async Task<ActionResult<StudentDTO>> UpdateStudent(string id, UpdateStudentDTO updateStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is not valid");
                var firstError = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

                return BadRequest(ApiResponse<string>.BadRequest(
                    error: new ApiError()
                    {
                        Code = firstError
                    }
                ));
            }
            var result = await _studentService.UpdateStudentAsync(id, updateStudentDTO);
            if (result.Success)
            {
                return Ok(ApiResponse<StudentDTO>.Success(
                    data: result.Data
                ));
            }
            return BadRequest(ApiResponse<string>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode
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
                        Code = res.ErrorCode
                    }
                ));
            }
        }
    }
}