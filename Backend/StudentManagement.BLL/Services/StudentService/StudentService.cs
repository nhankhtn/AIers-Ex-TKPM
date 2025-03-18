using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.Application.DTOs;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Utils;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IStudentStatusRepository _studentStatusRepository;
        private readonly IMapper _mapper;

        private readonly List<string> _requiredProperties = new List<string>
        {
            nameof(StudentDTO.Id),
            nameof(StudentDTO.Name),
            nameof(StudentDTO.DateOfBirth),
            nameof(StudentDTO.Gender),
            nameof(StudentDTO.Course),
            nameof(StudentDTO.Phone),
            nameof(StudentDTO.MailingAddress),
            nameof(StudentDTO.Program),
            nameof(StudentDTO.Status),
            nameof(StudentDTO.Faculty),
            nameof(StudentDTO.Nationality)
        };



        public StudentService(IStudentRepository studentRepository, 
            IFacultyRepository facultyRepository , IStudentStatusRepository studentStatusRepository, IProgramRepository programRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _programRepository = programRepository ?? throw new ArgumentNullException(nameof(programRepository));
            _studentStatusRepository = studentStatusRepository ?? throw new ArgumentNullException(nameof(studentStatusRepository));
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
        public async Task<Result<AddListStudentResult>> AddListStudentAsync(AddListStudentDTO addListStudentDTO)
        {
            //var result = new AddListStudentResult();
            //foreach (var student in addListStudentDTO.Students)
            //{
            //    if (CheckValidateStudent(student))
            //}
            throw new NotImplementedException();
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

            //foreach (var setter in SpecialSetters())
            //{
            //    var prop = typeof(UpdateStudentDTO).GetProperty(setter.Key);
            //    if (prop is null) return Result<StudentDTO>.Fail("SOMETHING WRONG");

            //    var value = prop.GetValue(updateStudentDTO);
            //    if (value is null) continue;

            //    var res = setter.Value(student, value);
            //    if (!res) return Result<StudentDTO>.Fail("INVALID_" + setter.Key.ToUpper());
            //}

            foreach (var prop in typeof(UpdateStudentDTO).GetProperties())
            {
               // if (SpecialSetters().ContainsKey(prop.Name)) continue;

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
        private Dictionary<string, Func<Student, object, bool>> specialSetters() => new()
        {
            { nameof(Student.Gender), (student, value) => SetEnumValue(value, out Gender gender) && (student.Gender = gender) == gender },
            { nameof(Student.Identity), (student, value) =>
                {
                    if (value is null) return true;
                    if (_mapper is null) return false;
                    student.Identity = _mapper.Map<Identity>(value);
                    return true;
                }},
            { nameof(Student.ProgramId), (student, value) => (student.ProgramId = ParseGuidSafely(value.ToString())) != Guid.Empty },
            { nameof(Student.FacultyId), (student, value) =>  (student.FacultyId = ParseGuidSafely(value.ToString())) != Guid.Empty},
            { nameof(Student.StatusId), (student, value) =>  (student.StatusId = ParseGuidSafely(value.ToString())) != Guid.Empty}
        };


        /// <summary>
        /// Validate properties
        /// </summary>
        /// <returns></returns>
        //private Dictionary<string, Func<AddStudentDTO, Task<bool>>> validateProperties() => new()
        //{
        //    { nameof(AddStudentDTO.Course), student => Task.FromResult(student.Course > 2000) },
        //    { nameof(AddStudentDTO.Email), student => Task.FromResult(student.Email is null ||
        //                                        Regex.IsMatch(student.Email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))},
        //    { nameof(AddStudentDTO.Phone), student => Task.FromResult(Regex.IsMatch(student.Phone, @"^0\d{9,10}$"))},
        //    { nameof(AddStudentDTO.FacultyId), async student => {
        //        if (_facultyRepository is null || student.FacultyId is null) return false;
        //        return await _facultyRepository.GetFacultyByNameAsync(student.Faculty.Name) is not null;
        //    } },
        //    { nameof(AddStudentDTO.ProgramId), async student => {
        //        if (_programRepository is null || student.ProgramId is null || student.Program.Name is null) return false;
        //        return await _programRepository.GetProgramByNameAsync(student.Program.Name) is not null;
        //    } },
        //    { nameof(AddStudentDTO.Status), async student => {
        //        if (_studentStatusRepository is null || student.Status is null || student.Status.Name is null) return false;
        //        return await _studentStatusRepository.GetStudentStatusByNameAsync(student.Status.Name) is not null;
        //    } },
        //};


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

        /// <summary>
        /// Parse guid safely
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Guid ParseGuidSafely(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return Guid.Empty; // or Guid.NewGuid()

            if (Guid.TryParse(input, out Guid result))
                return result;

            return Guid.Empty; // or Guid.NewGuid()
        }

        /// <summary>
        /// Check validate student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        private bool CheckValidateStudent(AddStudentDTO student)
        {
            foreach (var prop in _requiredProperties)
            { 
                if (typeof(AddStudentDTO).GetProperty(prop)?.GetValue(student) is null) return false;
            }
            //foreach (var prop in validateProperties())
            //{
            //    if (!prop.Value(student).Result) return false;
            //}
            return true;
        }
    }
}