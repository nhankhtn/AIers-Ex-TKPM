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
            {
                "Gender", (student, value) =>
                {
                    if (value is string genderStr && (new[] {"Male", "Female"}).Contains(value))
                    {
                        student.Gender = genderStr == "Male"; 
                        return true;
                    }
                    return false;
                }
            },
            {
                "Faculty", (student, value) =>
                {
                    var res = SetEnumValue<Student, Faculty>(student, "Faculty", value);
                    return res;
                }
            },
            {
                "Status", (student, value) =>
                {
                    var res = SetEnumValue<Student, StudentStatus>(student, "Status", value);
                    return res;
                }
            }
        };

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }


        // Get all students
        public async Task<Result<IEnumerable<StudentDTO>>> GetAllStudentsAsync(int page, int pageSize, string? name, string? id)
        {
            var students = await _studentRepository.GetAllStudentsAsync(page, pageSize, name, id);
            return Result<IEnumerable<StudentDTO>>.Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
        }


        // Add a new student
        public async Task<Result<string>> AddStudentAsync(StudentDTO studentDTO)
        {
            var newStudent = new Student();
            foreach (var setter in SpecialSetters)
            {
                var prop = typeof(StudentDTO).GetProperty(setter.Key);
                if (prop is null) return Result<string>.Fail("INVALID_" + setter.Key.ToUpper());

                var value = prop.GetValue(studentDTO);
                if (value is null) return Result<string>.Fail("INVALID_" + setter.Key.ToUpper());

                var res = setter.Value(newStudent, value);
                if (!res) return Result<string>.Fail("INVALID_" + setter.Key.ToUpper());
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
                    return Result<string>.Fail("INVALID_" + prop.Name.ToUpper());
                }
            }
            
            var result = await _studentRepository.AddStudentAsync(newStudent);
            return result ? Result<string>.Ok("ADD_SUCCESS") : Result<string>.Fail("ADD_FAIL");
        }


        // Delete a student
        public async Task<Result<string>> DeleteStudentAsync(string studentId)
        {
            var res = await _studentRepository.DeleteStudentAsync(studentId);
            return res ? Result<string>.Ok("DELETE_SUCCESS") : Result<string>.Fail("DELETE_FAIL");
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


        // Update a student
        public async Task<Result<string>> UpdateStudentAsync(string studentId, UpdateStudentDTO updateStudentDTO)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return Result<string>.Fail("STUDENT_NOT_FOUND");

            foreach (var setter in SpecialSetters)
            {
                var prop = typeof(UpdateStudentDTO).GetProperty(setter.Key);
                if (prop is null) return Result<string>.Fail("INVALID_" + setter.Key.ToUpper());

                var value = prop.GetValue(updateStudentDTO);
                if (value is null) continue;

                var res = setter.Value(student, value);
                if (!res) return Result<string>.Fail("INVALID_" + setter.Key.ToUpper());
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
                    return Result<string>.Fail("INVALID_" + prop.Name.ToUpper());
                }
            }

            var result = await _studentRepository.UpdateStudentAsync(student);
            return result ? Result<string>.Ok("UPDATE_SUCCESS") : Result<string>.Fail("UPDATE_FAIL");
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
        private static bool SetEnumValue<T, TEnum>(T obj, string propertyName, object value) where TEnum: struct, Enum
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