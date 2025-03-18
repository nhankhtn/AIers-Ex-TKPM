using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
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
        private readonly IMapper _mapper;

        // These properties can be null
        private readonly List<string> _requiredProperties = new List<string>
        {
            nameof(StudentDTO.Name),
            nameof(StudentDTO.DateOfBirth),
            nameof(StudentDTO.Gender),
            nameof(StudentDTO.Course),
            nameof(StudentDTO.Phone),
            nameof(StudentDTO.PermanentAddress),
            nameof(StudentDTO.Program),
            nameof(StudentDTO.Status),
            nameof(StudentDTO.Faculty),
            nameof(StudentDTO.Nationality),
            nameof(StudentDTO.Identity)
        };

        private readonly List<string> _canNotUpdatProperties = new List<string>() { nameof(StudentDTO.Id) };


        /// <summary>
        /// Validate properties
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, Func<StudentDTO, Task<bool>>> _validateValue = new() { };

        /// <summary>
        /// Special mapping
        /// </summary>
        private readonly Dictionary<string, Func<Student, object, Task<bool>>> _specialMapping = new() { };
        private readonly Dictionary<string, Func<StudentDTO, object, Task<bool>>> _specialMappingReverse = new() { };

        private Dictionary<int, int> generateIdCache = new Dictionary<int, int>();

        public StudentService(IStudentRepository studentRepository,
            IFacultyRepository facultyRepository, IStudentStatusRepository studentStatusRepository, IProgramRepository programRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _facultyRepository = facultyRepository ?? throw new ArgumentNullException(nameof(facultyRepository));
            _programRepository = programRepository ?? throw new ArgumentNullException(nameof(programRepository));
            _studentStatusRepository = studentStatusRepository ?? throw new ArgumentNullException(nameof(studentStatusRepository));
            _mapper = mapper;

            _validateValue = new()
            {
                // Validate email
                { nameof(StudentDTO.Email), async student
                    => student.Email is not null && Regex.IsMatch(student.Email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$") && (await _studentRepository.IsEmailDuplicateAsync(student.Email)).Success },

                // Validate phone
                { nameof(StudentDTO.Phone), student  => Task.FromResult(student.Phone is not null && Regex.IsMatch(student.Phone, @"^0\d{9,10}$")) },

                // Validate course
                
                { nameof(StudentDTO.Course), student => Task.FromResult(student.Course > 2000 && student.Course < 2100) },

                // Validate faculty
                { nameof(StudentDTO.Faculty), async student => student.Faculty is not null && _facultyRepository is not null
                && (await _facultyRepository.GetFacultyByNameAsync(student.Faculty)).Data is not null },

                // Validate program
                { nameof(StudentDTO.Program), async student => student.Program is not null 
                && (await _programRepository.GetProgramByNameAsync(student.Program)).Data is not null },
            
                // Validate status
                { nameof(StudentDTO.Status), async student => student.Status is not null 
                && (await _studentStatusRepository.GetStudentStatusByNameAsync(student.Status)).Data is not null },
            };

            _specialMapping = new()
            {
                { nameof(StudentDTO.Faculty), async (student, value) => {
                    if (value is not null && value is string facultyName )
                    {
                        var faculty = (await _facultyRepository.GetFacultyByNameAsync(facultyName)).Data;
                        if (faculty is not null)
                        {
                            student.FacultyId = faculty.Id;
                            student.Faculty = faculty;
                            return true;
                        }
                    }
                    return false;
                } },
                { nameof(StudentDTO.Program), async (student, value) => {
                    if (value is not null && value is string programName)
                    {
                        var program = (await _programRepository.GetProgramByNameAsync(programName)).Data;
                        if (program is not null)
                        {
                            student.ProgramId = program.Id;
                            student.Program = program;
                            return true;
                        }
                    }
                    return false;
                } },
                { nameof(StudentDTO.Status), async (student, value) => {
                    if (value is not null && value is string statusName)
                    {
                        var status = (await _studentStatusRepository.GetStudentStatusByNameAsync(statusName)).Data;
                        if (status is not null)
                        {
                            student.StatusId = status.Id;
                            student.Status = status;
                            return true;
                        }
                    }
                    return false;
                } },
            };

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
        public async Task<Result<AddListStudentResult>> AddListStudentAsync(IEnumerable<StudentDTO> studentDTOs)
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
                    if ((_requiredProperties.Contains(prop.Name) && value is null)
                        || (_validateValue.ContainsKey(prop.Name) && !(await _validateValue[prop.Name](student))))
                    {
                        result.UnacceptableStudents.Add(student);
                        validate = false;
                        break;
                    }
                    if (value is not null && _specialMapping.ContainsKey(prop.Name) && await _specialMapping[prop.Name](addStudent, value)) continue;
                }
                if (!validate) continue;

                var newId = await GenerateStudentId(student.Course ?? throw new ArgumentNullException());
                addStudent.Id = newId;
                addList.Add(addStudent);
            }
            var res = await _studentRepository.AddStudentAsync(addList);
            if (res.Success)
            {
                result.AcceptableStudents = _mapper.Map<IEnumerable<StudentDTO>>(addList).ToList();
                return Result<AddListStudentResult>.Ok(result);
            }
            return Result<AddListStudentResult>.Fail(res.ErrorCode, res.ErrorMessage);
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
        public async Task<Result<StudentDTO>> UpdateStudentAsync(string studentId, StudentDTO studentDTO)
        {
            var resExistStudent =  await _studentRepository.GetStudentByIdAsync(studentId);
            if (!resExistStudent.Success) return Result<StudentDTO>.Fail(resExistStudent.ErrorCode, resExistStudent.ErrorMessage);
            if (resExistStudent.Data is null) return Result<StudentDTO>.Fail("STUDENT_NOT_FOUND", "Student is not found");


            foreach(var prop in typeof(StudentDTO).GetProperties())
            {
                var value = prop.GetValue(studentDTO);
                if (_canNotUpdatProperties.Contains(prop.Name) || value is null) continue;
                if (_validateValue.ContainsKey(prop.Name) && !(await _validateValue[prop.Name](studentDTO))) return Result<StudentDTO>.Fail("INVALID_VALUE", "Invalid value");
                if (_specialMapping.ContainsKey(prop.Name) && await _specialMapping[prop.Name](resExistStudent.Data,value)) continue;

                var studentProp = typeof(Student).GetProperty(prop.Name);
                if (studentProp != null)
                {
                    studentProp.SetValue(resExistStudent.Data, prop.GetValue(studentDTO));
                }
            }
            
            var res = await _studentRepository.UpdateStudentAsync(_mapper.Map<Student>(resExistStudent.Data));
            return res.Success ? Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(resExistStudent.Data)) 
                : Result<StudentDTO>.Fail(res.ErrorCode, res.ErrorMessage);
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


            return $"{idCourseYear}{nextId:D4}";
        }
    }

}