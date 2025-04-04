﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<GetStudentsDTO>> GetAllStudents(int page, int limit, string? faculty, string? program, string? status, string? key)
        {
            var result = await _studentService.GetAllStudentsAsync(page, limit, faculty, program, status, key);
            if (result.Success)
            {
                if (result.Data is null) return NotFound(new ApiResponse<string>(
                    error: new ApiError() {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
                ));
                return Ok(result.Data);
            }
            return NotFound(new
            {
                error = new
                {
                    code = result.ErrorCode,
                    message = result.ErrorMessage
                }
            });
        }



        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddListStudentResult>>> AddStudent(IEnumerable<StudentDTO> studentDTOs)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

                return BadRequest(ApiResponse<AddListStudentResult>.BadRequest(
                    error: new ApiError()
                    {
                        Code = firstError
                    }
                ));
            }
            var result = await _studentService.AddListStudentAsync(studentDTOs);

            if (result.Success)
            {
                return StatusCode(207, ApiResponse<AddListStudentResult>.MultiStatus(
                    data: result.Data,
                    message: result.Message,
                    errors: result.Errors?.Select(e => new ApiError() { Index = e.index, Code = e.errorCode }).ToList()
                ));
            }
            return BadRequest(ApiResponse<AddListStudentResult>.BadRequest(
                    error: new ApiError()
                    {
                        Code = result.ErrorCode,
                        Message = result.ErrorMessage
                    }
            ));
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDTO>>> UpdateStudent(string id, StudentDTO updateStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is not valid");
                var firstError = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

                return BadRequest(ApiResponse<StudentDTO>.BadRequest(
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
            return BadRequest(ApiResponse<StudentDTO>.BadRequest(
                error: new ApiError()
                {
                    Code = result.ErrorCode,
                    Message = result.ErrorMessage
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
                        Code = res.ErrorCode,
                        Message = res.ErrorMessage
                    }
                ));
            }
        }
    }
}