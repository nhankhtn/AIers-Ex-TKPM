using Microsoft.AspNetCore.Mvc;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.Services;

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
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents(int page, int limit, string? key)
        {
            var result = await _studentService.GetAllStudentsAsync(page, limit, key);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    code = "STUDENT_NOT_FOUND"
                });
                return Ok(new
                {
                    title = "Success",
                    status = 200,
                    data = new
                    {
                        students = result.Data.Students,
                        total = result.Data.Total,
                        pageIndex = result.Data.PageIndex,
                        pageSize = result.Data.PageSize
                    }
                });
            }
            return NotFound(new
            {
                title = "Not Found",
                status = 404,
                code = result.ErrorCode
            });
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<StudentDTO>> GetStudentById(string id)
        //{
        //    var result = await _studentService.GetStudentByIdAsync(id);
        //    if (result.Success) return Ok(result.Data);
        //    return NotFound(new
        //    {
        //        title = "Not Found",
        //        status = 404,
        //        code = result.ErrorCode
        //    });
        //}


        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsByName(string name)
        //{
        //    var result = await _studentService.GetStudentsByNameAsync(name);
        //    if (result.Success) return Ok(result.Data);
        //    return NotFound(new
        //    {
        //        title = "Not Found",
        //        status = 404,
        //        code = result.ErrorCode
        //    });
        //}


        [HttpPost]
        public async Task<ActionResult<StudentDTO>> AddStudent(StudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    code = firstError
                });
            }
            var result = await _studentService.AddStudentAsync(studentDTO);

            if (result.Success)
            {
                return Ok(new { 
                    title = "Success",
                    status = 200,
                    data = new
                    {
                        Student = result.Data
                    }
                });
            }
            return BadRequest(new
            {
                title = "Bad Request",
                status = 400,
                code = result.ErrorCode
            });
        }




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

                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    code = firstError
                });
            }
            var result = await _studentService.UpdateStudentAsync(id, updateStudentDTO);
            if (result.Success)
            {
                return Ok(new
                {
                    title = "Success",
                    status = 200,
                    data = new { }
                });
            }
            return BadRequest(new
            {
                title = "Bad Request",
                status = 400,
                code = result.ErrorCode
            });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudentAsync(string id)
        {
            var res = await _studentService.DeleteStudentByIdAsync(id);
            if (res.Success)
            {
                return Ok(new
                {
                    title = "Success",
                    status = 200,
                    data = res.Data
                });
            }
            else
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    code = res.ErrorCode
                });
            }
        }
    }
}
