using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.Checker;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Validators;
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
using System.Diagnostics;
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
        private readonly IStudentValidator _userValidator;
        private readonly IStudentChecker _studentChecker;
        private readonly IMapper _mapper;

        private readonly List<string> _canNotUpdatProperties = new List<string>() { nameof(StudentDTO.Id) };

        private readonly Dictionary<string, Func<Student, object, bool>> _specialMapping = new() { };

        private Dictionary<int, int> generateIdCache = new Dictionary<int, int>();

        public StudentService(IStudentRepository studentRepository,
            IFacultyRepository facultyRepository, IStudentStatusRepository studentStatusRepository, IProgramRepository programRepository, IStudentValidator userValidator,
            IStudentChecker studentChecker,
            IMapper mapper)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _programRepository = programRepository ?? throw new ArgumentNullException(nameof(programRepository));
            _studentStatusRepository = studentStatusRepository ?? throw new ArgumentNullException(nameof(studentStatusRepository));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            _studentChecker = studentChecker ?? throw new ArgumentNullException(nameof(studentChecker));
            _mapper = mapper;

            _specialMapping = new()
            {
                { nameof(StudentDTO.Faculty), (student, value) => {
                    if (value is not null && value is string facultyId)
                    {
                        student.FacultyId = facultyId.ToGuid();
                        return true;
                    }
                    return false;
                } },
                { nameof(StudentDTO.Program), (student, value) => {
                    if (value is not null && value is string programId)
                    {
                        student.ProgramId = programId.ToGuid();
                        return true;
                    }
                    return false;
                } },
                { nameof(StudentDTO.Status), (student, value) => {
                    if (value is not null && value is string statusId)
                    {
                        student.StatusId = statusId.ToGuid();
                        return true;
                    }
                    return false;
                } },

                { nameof(StudentDTO.Gender), (student, value) => {
                    if (SetEnumValue(value, out Gender gender))
                    {
                        student.Gender = gender;
                        return true;
                    }
                    return false;
                } },

                { nameof(Student.Identity), (student, typeValue) =>
                {
                    if (typeValue is not null && typeValue is IdentityDTO identityDTO)
                    {
                        student.Identity.Type = identityDTO.Type.ToEnum<IdentityType>();
                        return true;
                    }
                    return false;
                } }
            };
        }



        // Get all students
        public async Task<Result<GetStudentsDTO>> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? program, string? status, string? key)
        {
            try
            {
                var res = await _studentRepository.GetAllStudentsAsync(page, pageSize, faculty, program, status, key);
                return Result<GetStudentsDTO>.Ok(new GetStudentsDTO()
                {
                    Students = _mapper.Map<IEnumerable<StudentDTO>>(res.students),
                    Total = res.total
                });
            }
            catch (Exception ex)
            {
                return Result<GetStudentsDTO>.Fail("500", ex.Message);
            }
        }


        // Add new students
        public async Task<Result<AddListStudentResult>> AddListStudentAsync(IEnumerable<StudentDTO> studentDTOs)
        {
            try
            {
                var result = new AddListStudentResult();
                var addList = new List<Student>();
                foreach (var student in studentDTOs)
                {
                    var validate = true;
                    var addStudent = _mapper.Map<Student>(student);
                    foreach (var prop in typeof(StudentDTO).GetProperties())
                    {
                        var value = prop.GetValue(student);
                        if ((StudentDTO.RequiredProperties.Contains(prop.Name) && value is null)
                            || (_userValidator.NeedValidateProperties.Contains(prop.Name) && !(_userValidator.StudentValidate(prop.Name, student)))
                            || (_studentChecker.NeedCheckedProperties.Contains(prop.Name) && !(await _studentChecker.StudentChecked(prop.Name, student))))
                        {
                            result.UnacceptableStudents.Add(student);
                            validate = false;
                            break;
                        }
                        if (value is not null && _specialMapping.ContainsKey(prop.Name) && _specialMapping[prop.Name](addStudent, value)) continue;
                    }
                    if (!validate) continue;

                    var newId = await GenerateStudentId(student.Course ?? throw new ArgumentNullException());
                    addStudent.Id = newId;
                    addList.Add(addStudent);
                }
                var res = await _studentRepository.AddStudentAsync(addList);
                result.AcceptableStudents = _mapper.Map<IEnumerable<StudentDTO>>(addList).ToList();
                return Result<AddListStudentResult>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result<AddListStudentResult>.Fail("500", ex.Message);
            }
            
        }



        // Get student by id
        public async Task<Result<StudentDTO>> GetStudentByIdAsync(string studentId)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIdAsync(studentId);
                return student != null
                    ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(student))
                    : Result<StudentDTO>.Fail("STUDENT_NOT_FOUND");
            }
            catch (Exception ex)
            {
                return Result<StudentDTO>.Fail("500", ex.Message);
            }
            
        }


        // Get students by name
        public async Task<Result<IEnumerable<StudentDTO>>> GetStudentsByNameAsync(string name)
        {
            try
            {
                var students = await _studentRepository.GetStudentsByNameAsync(name);
                return Result<IEnumerable<StudentDTO>>.Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<StudentDTO>>.Fail("500", ex.Message);
            }
        }


        /// <summary>
        /// Delete a student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<Result<string>> DeleteStudentByIdAsync(string studentId)
        {
            try
            {
                await _studentRepository.DeleteStudentAsync(studentId);
                return Result<string>.Ok(studentId);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("500", ex.Message);
            }
        }


        // Update a student
        public async Task<Result<StudentDTO>> UpdateStudentAsync(string studentId, StudentDTO studentDTO)
        {
            try
            {
                var resExistStudent = await _studentRepository.GetStudentByIdAsync(studentId);
                if (resExistStudent is null) return Result<StudentDTO>.Fail("STUDENT_NOT_FOUND", "Student is not found");

                foreach (var prop in typeof(StudentDTO).GetProperties())
                {
                    var value = prop.GetValue(studentDTO);
                    if (_canNotUpdatProperties.Contains(prop.Name) || value is null) continue;

                    if (_userValidator.NeedValidateProperties.Contains(prop.Name) && !(_userValidator.StudentValidate(prop.Name, studentDTO))) 
                        return Result<StudentDTO>.Fail("INVALID_VALUE", "Invalid value");

                    if (_studentChecker.NeedCheckedProperties.Contains(prop.Name) && !(await _studentChecker.StudentChecked(prop.Name, studentDTO)))
                        return Result<StudentDTO>.Fail($"DUPLICATE_{prop.Name.ToUpper()}", "Duplicate value");

                    if (_specialMapping.ContainsKey(prop.Name) && _specialMapping[prop.Name](resExistStudent, value)) continue;

                    var studentProp = typeof(Student).GetProperty(prop.Name);
                    if (studentProp != null)
                    {
                        studentProp.SetValue(resExistStudent, prop.GetValue(studentDTO));
                    }
                }

                var res = await _studentRepository.UpdateStudentAsync(_mapper.Map<Student>(resExistStudent));
                return Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(resExistStudent));
            }
            catch (Exception ex)
            {
                return Result<StudentDTO>.Fail("500", ex.Message);
            }
        }



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
            if (value is string strValue && Enum.TryParse<T>(strValue, true, out var enumValue))
            {
                result = enumValue;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Generate student ID
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        private async Task<string> GenerateStudentId(int course)
        {
            var idCourseYear = course % 100;
            int nextId;
            if (generateIdCache.ContainsKey(idCourseYear))
            {
                nextId = generateIdCache[idCourseYear] + 1;
            }
            else
            {
                var lastId = await _studentRepository.GetLatestStudentIdAsync(course);
                nextId = lastId + 1;
            }
            generateIdCache[idCourseYear] = nextId;

            return $"{idCourseYear}{nextId:D4}";
        }
    }

}