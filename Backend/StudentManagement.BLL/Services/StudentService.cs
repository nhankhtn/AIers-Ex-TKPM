using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, Func<Student, object, bool>> SpecialSetters = new()
        {
            { "Gender", (student, value) => SetEnumValue(value, out Gender gender) && (student.Gender = gender) == gender },
            { "Faculty", (student, value) => SetEnumValue(value, out Faculty faculty) && (student.Faculty = faculty) == faculty },
            { "Status", (student, value) => SetEnumValue(value, out StudentStatus status) && (student.Status = status) == status }
        };

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


        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }


        // Get all students
        public async Task<Result<GetStudentsDto>> GetAllStudentsAsync(int page, int pageSize, string? key)
        {
            var students = await _studentRepository.GetAllStudentsAsync(page, pageSize, key);

            return Result<GetStudentsDto>.Ok(new GetStudentsDto()
            {
                Students = _mapper.Map<IEnumerable<StudentDTO>>(students.students),
                Total = students.total,
                PageIndex = page,
                PageSize = pageSize
            });
        }


        // Add a new student
        public async Task<Result<StudentDTO>> AddStudentAsync(StudentDTO studentDTO)
        {
            var newStudent = new Student();
            foreach (var setter in SpecialSetters)
            {
                var prop = typeof(StudentDTO).GetProperty(setter.Key);
                if (prop is null) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());

                var value = prop.GetValue(studentDTO);
                if (value is null) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());

                //// Nếu là string, cố gắng chuyển đổi thành int
                //if (value is string strValue && int.TryParse(strValue, out var intValue))
                //{
                //    value = intValue; // Chuyển thành int để xử lý tiếp
                //}

                var res = setter.Value(newStudent, value);
                if (!res) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());
            }

            foreach (var prop in typeof(StudentDTO).GetProperties())
            {
                if (SpecialSetters.ContainsKey(prop.Name)) continue;

                var studentProp = typeof(Student).GetProperty(prop.Name);
                if (studentProp != null && studentProp.CanWrite)
                {
                    var value = prop.GetValue(studentDTO);
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

            if (newStudent.Email is not null && await _studentRepository.IsEmailExistAsync(newStudent.Email))
            {
                return Result<StudentDTO>.Fail("EMAIL_EXISTED");
            }

            var result = await _studentRepository.AddStudentAsync(newStudent);
            return result ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(newStudent)) : Result<StudentDTO>.Fail("ADD_FAIL");
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
            return res ? Result<string>.Ok(studentId) : Result<string>.Fail("DELETE_FAIL");
        }


        // Update a student
        public async Task<Result<StudentDTO>> UpdateStudentAsync(string studentId, UpdateStudentDTO updateStudentDTO)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return Result<StudentDTO>.Fail("STUDENT_NOT_FOUND");

            foreach (var setter in SpecialSetters)
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
                if (SpecialSetters.ContainsKey(prop.Name)) continue;

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

            if (student.Email is not null && await _studentRepository.IsEmailExistAsync(student.Email))
            {
                return Result<StudentDTO>.Fail("EMAIL_EXISTED");
            }

            var result = await _studentRepository.UpdateStudentAsync(student);
            return result ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(student)) : Result<StudentDTO>.Fail("UPDATE_FAIL");
        }


        /// <summary>
        /// Set value for a Enum property of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool SetEnumValue<T, TEnum>(T obj, string propertyName, object value) where TEnum : struct, Enum
        {
            if (value is string strValue && Enum.TryParse(strValue, out TEnum enumValue))
            {
                var objProp = typeof(T).GetProperty(propertyName);
                if (objProp != null && objProp.CanWrite)
                {
                    objProp.SetValue(obj, enumValue);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}