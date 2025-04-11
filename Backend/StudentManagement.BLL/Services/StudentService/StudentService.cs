using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using StudentManagement.BLL.Checker;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Students;
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

        private Dictionary<int, int> generateIdCache = new Dictionary<int, int>();

        public StudentService(IStudentRepository studentRepository,
            IFacultyRepository facultyRepository, 
            IStudentStatusRepository studentStatusRepository, 
            IProgramRepository programRepository, 
            IStudentValidator userValidator,
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
        }



        // Get all students
        public async Task<Result<GetStudentsDTO>> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? program, string? status, string? key)
        {
            try
            {
                var res = await _studentRepository.GetAllStudentsAsync(page, pageSize, faculty, program, status, key);
                return Result<GetStudentsDTO>.Ok(new GetStudentsDTO()
                {
                    Data = _mapper.Map<IEnumerable<StudentDTO>>(res.students),
                    Total = res.total
                });
            }
            catch (Exception ex)
            {
                return Result<GetStudentsDTO>.Fail("GET_STUDENTS_FAILED", ex.Message);
            }
        }


        // Add new students
        public async Task<Result<IEnumerable<StudentDTO>>> AddListStudentAsync(IEnumerable<StudentDTO> studentDTOs)
        {
            try
            {
                bool allValid = true;
                int index = -1;
                var errors = new List<(string errorCode, int index)>();
                foreach (var student in studentDTOs)
                {
                    index++;
                    var validRes = _userValidator.StudentValidate(student);
                    if (!validRes.IsValid)
                    {
                        allValid = false;
                        errors.Add((validRes.ErrorCode, index));
                        continue;
                    }
                    var checkRes = await _studentChecker.StudentCheckAsync(student);
                    if (!checkRes.IsValid)
                    {
                        allValid = false;
                        errors.Add((checkRes.ErrorCode, index));
                        continue;
                    }
                }
                if (!allValid)
                {
                    var errorMessages = errors.Select(e => $"Error at index {e.index}: {e.errorCode}").ToList();
                    return Result<IEnumerable<StudentDTO>>.Fail("ADD_STUDENTS_FAILED", null, errors);
                }

                // Generate Id for all
                foreach(var student in studentDTOs)
                {
                    if (student.Id is null)
                    {
                        var id = await GenerateStudentId(student.Course ?? throw new ArgumentNullException("Course is required."));
                        student.Id = id;
                    }
                }

                await _studentRepository.AddStudentAsync(_mapper.Map<IEnumerable<Student>>(studentDTOs));
                return Result<IEnumerable<StudentDTO>>.Ok(studentDTOs);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<StudentDTO>>.Fail("ADD_STUDENTS_FAILED", ex.Message);
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
                return Result<StudentDTO>.Fail("GET_STUDENT_FAILED", ex.Message);
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
                return Result<IEnumerable<StudentDTO>>.Fail("GET_STUDENT_FAILED", ex.Message);
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
                return Result<string>.Fail("DELETE_STUDENT_FAILED", ex.Message);
            }
        }


        // Update a student
        public async Task<Result<StudentDTO>> UpdateStudentAsync(string studentId, StudentDTO studentDTO)
        {
            try
            {
                studentDTO.Id = studentId;
                var validRes = _userValidator.StudentValidate(studentDTO, true);
                if (!validRes.IsValid)
                {
                    return Result<StudentDTO>.Fail(validRes.ErrorCode);
                }
                var checkRes = await _studentChecker.StudentCheckAsync(studentDTO, true);
                if (!checkRes.IsValid)
                {
                    return Result<StudentDTO>.Fail(checkRes.ErrorCode);
                }

                var resExistStudent = await _studentRepository.GetStudentByIdAsync(studentId);
                if (resExistStudent is null) return Result<StudentDTO>.Fail("STUDENT_NOT_FOUND", "Student is not found");

                _mapper.Map(studentDTO, resExistStudent);

                var res = await _studentRepository.UpdateStudentAsync(resExistStudent);
                return Result<StudentDTO>.Ok(_mapper.Map<StudentDTO>(resExistStudent));
            }
            catch (Exception ex)
            {
                return Result<StudentDTO>.Fail("UPDATE_STUDENT_FAILED", ex.Message);
            }
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