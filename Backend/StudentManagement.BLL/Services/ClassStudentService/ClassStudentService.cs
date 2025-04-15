using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Score;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.DAL.Data.Repositories.ClassRepo;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassStudentService
{
    public class ClassStudentService : IClassStudentService
    {
        private readonly IClassStudentRepository _classStudentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRegisterCancellationHistoryRepository _registerCancellationHistoryRepository;
        private readonly IMapper _mapper;

        public ClassStudentService(
            IClassStudentRepository classStudentRepository, 
            IStudentRepository studentRepository, 
            IClassRepository classRepository, 
            IRegisterCancellationHistoryRepository registerCancellationHistoryRepository,
            IMapper mapper)
        {
            _classStudentRepository = classStudentRepository ?? throw new ArgumentNullException(nameof(classStudentRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _registerCancellationHistoryRepository = registerCancellationHistoryRepository ?? throw new ArgumentNullException(nameof(registerCancellationHistoryRepository));
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<GetClassStudentDTO>>> AddStudentAsync(string studentId, IEnumerable<string> classIds)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIdAsync(studentId);
                if (student == null)
                    return Result<IEnumerable<GetClassStudentDTO>>.Fail("STUDENT_NOT_FOUND", ErrorMessages.StudentNotFound);

                var registerSuccessfulClasses = new List<GetClassStudentDTO>();
                var errors = new List<(string errorCode, string? errorMessage, int index)>();
                int index = -1;

                foreach (var classId in classIds)
                {
                    index++;
                    var _class = await _classRepository.GetClassByIdAsync(classId);
                    if (_class == null || _class.Deadline < DateTime.Now)
                    {
                        errors.Add(("CLASS_NOT_FOUND", $"Lớp {classId} không tồn tại hoặc đã hết hạn đăng ký.", index));
                        continue;
                    }
                    if (await _classStudentRepository.GetClassStudentByIdAsync(classId, studentId) != null)
                    {
                        errors.Add(("STUDENT_ALREADY_JOINED_CLASS", $"Sinh viên đã tham gia lớp {classId}.", index));
                        continue;
                    };
                    var requiredPreCourse = _class.Course.RequiredCourseId;
                    if (requiredPreCourse != null)
                    {
                        var studentJoinedClass = await _classStudentRepository.GetClassStudentByIdAndCourseAsync(studentId, requiredPreCourse);
                        bool isPassed = false;
                        foreach(var c in studentJoinedClass)
                        {
                            if (c == null) continue;
                            if (c.TotalScore >= 4)
                            {
                                isPassed = true;
                                break;
                            }
                        }
                        if (isPassed) registerSuccessfulClasses.Add(new GetClassStudentDTO()
                        {
                            StudentId = studentId,
                            ClassId = classId,
                            CourseName = await _classRepository.GetCourseNameAsync(classId),
                            StudentName = await _studentRepository.GetStudentNameAsync(studentId)
                        });
                        else
                        {
                            errors.Add(("STUDENT_NOT_PASSED_PRE_COURSE", $"Đăng ký lớp {_class.ClassId} thất bại. Sinh viên chưa qua môn tiên quyết.", index));
                        }
                    }
                    else
                    {
                        registerSuccessfulClasses.Add(new GetClassStudentDTO()
                        {
                            StudentId = studentId,
                            ClassId = classId,
                            CourseName = await _classRepository.GetCourseNameAsync(classId),
                            StudentName = await _studentRepository.GetStudentNameAsync(studentId)
                        });
                    }
                    var classStudent = await _classStudentRepository.AddClassStudentAsync(new ClassStudent()
                    {
                        StudentId = studentId,
                        ClassId = classId,
                        MidTermScore = 0,
                        FinalScore = 0,
                        TotalScore = 0,
                        IsPassed = false,
                    });
                }

                return Result<IEnumerable<GetClassStudentDTO>>.Multi(registerSuccessfulClasses, null, null, errors);
            }
            catch(DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("PK_class_student"))
            {
                return Result<IEnumerable<GetClassStudentDTO>>.Fail("DUPLICATE_CLASS_STUDENT_ID", ErrorMessages.DuplicateClassStudent);
            }
            catch(Exception)
            {
                return Result<IEnumerable<GetClassStudentDTO>>.Fail("ADD_STUDENT_TO_CLASS_FAILED", ErrorMessages.RegisterFailed);
            }
        }

        public async Task<Result<RegisterCancelationDTO>> RegisterCancelationAsync(RegisterCancelationDTO registerCancelationDTO)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(registerCancelationDTO.ClassId);
                if (_class is null)
                    return Result<RegisterCancelationDTO>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);

                var student = await _studentRepository.GetStudentByIdAsync(registerCancelationDTO.StudentId);
                if (student is null)
                    return Result<RegisterCancelationDTO>.Fail("STUDENT_NOT_FOUND", ErrorMessages.StudentNotFound);

                var classStudent = await _classStudentRepository.GetClassStudentsAsync(registerCancelationDTO.ClassId, registerCancelationDTO.StudentId, 1, 1);
                if (classStudent.Count() == 0)
                    return Result<RegisterCancelationDTO>.Fail("STUDENT_NOT_JOINED_CLASS_YET", "Sinh viên chưa đăng ký lớp này.");

                var registerCancellationHistory = _mapper.Map<RegisterCancellationHistory>(registerCancelationDTO);
                registerCancellationHistory.Time = DateTime.Now;
                registerCancellationHistory.Semester = _class.Semester;
                registerCancellationHistory.StudentName = await _studentRepository.GetStudentNameAsync(registerCancelationDTO.StudentId);
                registerCancellationHistory.AcademicYear = _class.AcademicYear;
                registerCancellationHistory.CourseName = await _classRepository.GetCourseNameAsync(registerCancelationDTO.ClassId);

                await _registerCancellationHistoryRepository.AddAsync(registerCancellationHistory);
                await _classStudentRepository.DeleteClassStudentAsync(registerCancelationDTO.ClassId, registerCancelationDTO.StudentId);
                return Result<RegisterCancelationDTO>.Ok(registerCancelationDTO);
            }
            catch (Exception)
            {
                return Result<RegisterCancelationDTO>.Fail("REGISTER_CANCELATION_FAILED", ErrorMessages.RegisterCancellationFailed);
            }
        }

        public async Task<Result<GetClassStudentsDTO>> GetClassStudentsAsync(string? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            try
            {
                var res = await _classStudentRepository.GetClassStudentsAsync(classId, studentId, page, limit);

                var data = _mapper.Map<IEnumerable<GetClassStudentDTO>>(res);
                foreach(var c in data)
                {
                    c.CourseName = await _classRepository.GetCourseNameAsync(c.ClassId);
                    c.StudentName = await _studentRepository.GetStudentNameAsync(c.StudentId);
                }

                return Result<GetClassStudentsDTO>.Ok(new GetClassStudentsDTO()
                {
                    Data = data,
                    Total = res.Count()
                });
            }
            catch (Exception)
            {
                return Result<GetClassStudentsDTO>.Fail("GET_ALL_CLASS_STUDENTS_FAILED", "Lấy danh sách tham gia lớp học thất bại.");
            }
        }

        public async Task<Result<IEnumerable<GetScoreDTO>>> UpdateScoresAsync(string classId, IEnumerable<UpdateScoreDTO> updateScoresDTO)
        {
            try
            {
                var result = new List<GetScoreDTO>();
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<IEnumerable<GetScoreDTO>>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);

                foreach (var updateScoreDTO in updateScoresDTO)
                {
                    var classStudent = await _classStudentRepository.GetClassStudentByIdAsync(_class.ClassId, updateScoreDTO.StudentId);
                    if (classStudent is null) continue;

                    _mapper.Map(updateScoreDTO, classStudent);
                    classStudent.TotalScore = classStudent.FinalScore * 0.6 + classStudent.MidTermScore * 0.4;
                    classStudent.IsPassed = classStudent.TotalScore >= 4;

                    if (classStudent.TotalScore < 4) classStudent.Grade = 'F';
                    else if (classStudent.TotalScore < 5.5) classStudent.Grade = 'D';
                    else if (classStudent.TotalScore < 7) classStudent.Grade = 'C';
                    else if (classStudent.TotalScore < 8.5) classStudent.Grade = 'B';
                    else classStudent.Grade = 'A';

                    await _classStudentRepository.UpdateClassStudentAsync(classStudent);

                    var cs = _mapper.Map<GetScoreDTO>(classStudent);
                    cs.StudentName = await _studentRepository.GetStudentNameAsync(cs.StudentId);
                    result.Add(cs);
                }
                
                return Result<IEnumerable<GetScoreDTO>>.Ok(result);
            }
            catch (Exception)
            {
                return Result<IEnumerable<GetScoreDTO>>.Fail("UPDATE_CLASS_STUDENTS_FAILED", "Cập nhật điểm số thất bại.");
            }
        }

        public async Task<Result<IEnumerable<GetScoreDTO>>> GetScoresAsync(string classId)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<IEnumerable<GetScoreDTO>>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);
                var allStudentInClass = await _classStudentRepository.GetClassStudentsAsync(classId, null, null, null);
                var data = _mapper.Map<IEnumerable<GetScoreDTO>>(allStudentInClass);
                foreach(var c in data)
                {
                    c.StudentName = await _studentRepository.GetStudentNameAsync(c.StudentId);
                }
                return Result<IEnumerable<GetScoreDTO>>.Ok(data);
            }
            catch (Exception)
            {
                return Result<IEnumerable<GetScoreDTO>>.Fail("GET_SCORES_FAILED", "Lấy bảng điểm thất bại.");
            }
        }

        public async Task<Result<StudentTranscriptDTO>> GetStudentTranscriptAsync(string studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) 
                return Result<StudentTranscriptDTO>.Fail("STUDENT_NOT_FOUND", ErrorMessages.StudentNotFound);

            var allJoinedClasses = await _classStudentRepository.GetClassStudentsAsync(null, studentId, null, null);

            int totalCredit = 0;
            int passedCredit = 0;
            double totalScore = 0;
            var data = new List<TranscriptRow>();

            foreach(var c in allJoinedClasses)
            {
                int credit = await _classRepository.GetCreditsAsync(c.ClassId);
                totalCredit += credit;
                if (c.IsPassed)
                {
                    passedCredit += credit;
                    totalScore += c.TotalScore * credit;
                }
                data.Add(new TranscriptRow()
                {
                    ClassId = c.ClassId,
                    CourseName = await _classRepository.GetCourseNameAsync(c.ClassId),
                    Credit = credit,
                    TotalScore = Math.Round(c.TotalScore, 2),
                    Grade = c.Grade
                });
            }

            return Result<StudentTranscriptDTO>.Ok(new StudentTranscriptDTO()
            {
                Transcript = data,
                StudentId = student.Id,
                StudentName = student.Name,
                FacultyName = student.Faculty.Name,
                Course = student.Course,
                TotalCredit = totalCredit,
                PassedCredit = passedCredit,
                GPA = passedCredit != 0 ? Math.Round(totalScore / passedCredit, 2) : 0
            });
        }

        public async Task<Result<IEnumerable<GetClassDTO>>> GetRegisterableClassesAsync(string studentId)
        {
            try
            {
                var allClasses = await _classRepository.GetClassesAsync(null, null, null, null);
                var registerableClasses = new List<GetClassDTO>();

                foreach (var c in allClasses)
                {
                    if (c.Deadline < DateTime.Now) continue; 
                    var classStudent = await _classStudentRepository.GetClassStudentByIdAsync(c.ClassId, studentId); 
                    if (classStudent != null) continue;
                    var requiredPreCourse = c.Course.RequiredCourseId;
                    if (requiredPreCourse != null)
                    {
                        var studentJoinedClasses = await _classStudentRepository.GetClassStudentByIdAndCourseAsync(studentId, requiredPreCourse);
                        bool isPassed = false;
                        foreach (var _c in studentJoinedClasses)
                        {
                            if (_c == null) continue;
                            if (_c.TotalScore >= 4)
                            {
                                isPassed = true;
                                break;
                            }
                        }
                        if (!isPassed) continue;
                        var classDTO = _mapper.Map<GetClassDTO>(c);
                        classDTO.CurrentStudents = await _classStudentRepository.GetNumberOfStudentsInClassAsync(c.ClassId);
                        registerableClasses.Add(classDTO);
                    }
                    else
                    {
                        var classDTO = _mapper.Map<GetClassDTO>(c);
                        classDTO.CurrentStudents = await _classStudentRepository.GetNumberOfStudentsInClassAsync(c.ClassId);
                        registerableClasses.Add(classDTO);
                    }
                }
                return Result<IEnumerable<GetClassDTO>>.Ok(registerableClasses);
            }
            catch(Exception)
            {
                return Result<IEnumerable<GetClassDTO>>.Fail("GET_REGISTERABLE_CLASSES_FAILED", "Lấy danh sách các môn có thể đăng ký thất bại.");
            }
        }

        public async Task<Result<GetRegisterCancelationHistoryDTO>> GetRegisterCancelationHistoryAsync(int? page, int? limit, string? key)
        {
            try
            {
                var res = await _registerCancellationHistoryRepository.GetAllAsync(page, limit, key);
                var data = _mapper.Map<IEnumerable<RegisterCancelationHistoryRow>>(res);
                return Result<GetRegisterCancelationHistoryDTO>.Ok(new GetRegisterCancelationHistoryDTO()
                {
                    Data = data,
                    Total = res.Count()
                });
            }
            catch (Exception)
            {
                return Result<GetRegisterCancelationHistoryDTO>.Fail("GET_REGISTER_CANCELATION_HISTORY_FAILED", "Lấy lịch sử đăng ký hủy lớp thất bại.");
            }
        }
    }
}
