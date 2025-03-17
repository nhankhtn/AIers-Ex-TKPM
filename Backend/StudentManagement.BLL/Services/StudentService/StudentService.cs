using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.Application.DTOs;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }


        // Get all students
        public async Task<Result<GetStudentsDTO>> GetAllStudentsAsync(int page, int pageSize, string? key)
        {
            var res = await _studentRepository.GetAllStudentsAsync(page, pageSize, key);
            if (!res.Success) return Result<GetStudentsDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<GetStudentsDTO>.Ok(new GetStudentsDTO()
            {
                Students = _mapper.Map<IEnumerable<StudentDTO>>(res.Data.students),
                Total = res.Data.total
            });
        }


        // Add a new student
        public async Task<Result<StudentDTO>> AddStudentAsync(AddStudentDTO addStudentDTO)
        {
            var newStudent = new Student();
            foreach (var setter in SpecialSetters())
            {
                var prop = typeof(AddStudentDTO).GetProperty(setter.Key);
                if (prop is null) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());

                var value = prop.GetValue(addStudentDTO);
                if (value is null && Nullable.GetUnderlyingType(prop.PropertyType) != null)
                    return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());

                if (value is null) continue;
                var res = setter.Value(newStudent, value);
                if (!res) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());
            }

            foreach (var prop in typeof(AddStudentDTO).GetProperties())
            {
                if (SpecialSetters().ContainsKey(prop.Name)) continue;

                var studentProp = typeof(Student).GetProperty(prop.Name);
                if (studentProp != null && studentProp.CanWrite)
                {
                    var value = prop.GetValue(addStudentDTO);
                    if (value != null)
                    {
                        studentProp.SetValue(newStudent, value);
                    }
                }
                else
                {
                    return Result<StudentDTO>.Fail("INVALID_" + prop.Name.ToUpper());
                }
            }

            var result = await _studentRepository.AddStudentAsync(newStudent);
            return result.Success ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(result.Data)) 
                : Result<StudentDTO>.Fail(result.ErrorCode, result.ErrorMessage);
        }



        // Get student by id
        public async Task<Result<StudentDTO>> GetStudentByIdAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            return student != null
                ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(student))
                : Result<StudentDTO>.Fail("STUDENT_NOT_FOUND");
        }


        // Get students by name
        public async Task<Result<IEnumerable<StudentDTO>>> GetStudentsByNameAsync(string name)
        {
            var students = await _studentRepository.GetStudentsByNameAsync(name);
            return Result<IEnumerable<StudentDTO>>.Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
        }


        /// <summary>
        /// Delete a student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<Result<string>> DeleteStudentByIdAsync(string studentId)
        {
            var res = await _studentRepository.DeleteStudentAsync(studentId);
            return res.Success ? Result<string>.Ok(studentId) : Result<string>.Fail(res.ErrorCode, res.ErrorMessage);
        }


        // Update a student
        public async Task<Result<StudentDTO>> UpdateStudentAsync(string studentId, UpdateStudentDTO updateStudentDTO)
        {
            var _res = await _studentRepository.GetStudentByIdAsync(studentId);
            if (!_res.Success) return Result<StudentDTO>.Fail(_res.ErrorCode, _res.ErrorMessage);
            if (_res.Data is null) return Result<StudentDTO>.Fail("NOT_FOUND_STUDENT", "Not found student");

            var student = _res.Data;

            foreach (var setter in SpecialSetters())
            {
                var prop = typeof(UpdateStudentDTO).GetProperty(setter.Key);
                if (prop is null) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());

                var value = prop.GetValue(updateStudentDTO);
                if (value is null) continue;

                var res = setter.Value(student, value);
                if (!res) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());
            }

            foreach (var prop in typeof(UpdateStudentDTO).GetProperties())
            {
                if (SpecialSetters().ContainsKey(prop.Name)) continue;

                var studentProp = typeof(Student).GetProperty(prop.Name);
                if (studentProp != null && studentProp.CanWrite)
                {
                    var value = prop.GetValue(updateStudentDTO);
                    if (value != null)
                    {
                        studentProp.SetValue(student, value);
                    }
                }
                else
                {
                    return Result<StudentDTO>.Fail("INVALID_" + prop.Name.ToUpper());
                }
            }

            var result = await _studentRepository.UpdateStudentAsync(student);
            return result.Success ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(student)) 
                : Result<StudentDTO>.Fail(result.ErrorCode, result.ErrorMessage);
        }



        /// <summary>
        /// Mapper for special setters
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Func<Student, object, bool>> SpecialSetters() => new()
        {
            { "Gender", (student, value) => SetEnumValue(value, out Gender gender) && (student.Gender = gender) == gender },
            { "Course", (student, value) => (value is int intValue && intValue > 2000) },
            { "PermanentAddress", (student, value) =>
                {
                    if (value is null) return true;
                    if (_mapper is null) return false;
                    student.PermanentAddress = _mapper.Map<Address>(value);
                    return true;
                }},
            { "Identity", (student, value) =>
                {
                    if (value is null) return true;
                    if (_mapper is null) return false;
                    student.Identity = _mapper.Map<Identity>(value);
                    return true;
                }},
            { "Nationalities", (student, value) =>
                {
                    if (value is null) return true;
                    if (_mapper is null) return false;
                    student.Nationalities = _mapper.Map<StudentNationalities>(value);
                    return true;
                }},
            { "ProgramId", (student, value) => (student.ProgramId = ParseGuidSafely(value.ToString())) != Guid.Empty },
            { "FacultyId", (student, value) =>  (student.FacultyId = ParseGuidSafely(value.ToString())) != Guid.Empty},
            { "StatusId", (student, value) =>  (student.StatusId = ParseGuidSafely(value.ToString())) != Guid.Empty}
        };


        /// <summary>
        /// Set enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool SetEnumValue<T>(object value, out T result) where T : struct, Enum
        {
            if (value is int intValue && Enum.IsDefined(typeof(T), intValue))
            {
                result = (T)(object)intValue;
                return true;
            }
            result = default;
            return false;
        }

        private Guid ParseGuidSafely(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return Guid.Empty; // or Guid.NewGuid()

            if (Guid.TryParse(input, out Guid result))
                return result;

            return Guid.Empty; // or Guid.NewGuid()
        }

    }
}