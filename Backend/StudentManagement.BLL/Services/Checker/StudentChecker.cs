using StudentManagement.BLL.DTOs.Students;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.Checker
{
    public class StudentChecker : IStudentChecker
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IStudentStatusRepository _studentStatusRepository;

        public List<string> NeedCheckedProperties { get; } = new List<string>() {
            nameof(StudentDTO.Email),
            nameof(StudentDTO.Phone),
            nameof(StudentDTO.Faculty),
            nameof(StudentDTO.Program),
            nameof(StudentDTO.Status),
            nameof(StudentDTO.Identity)
        };

        public StudentChecker(IStudentRepository studentRepository, IFacultyRepository facultyRepository, IProgramRepository programRepository, IStudentStatusRepository studentStatusRepository)
        {
            _studentRepository = studentRepository;
            _facultyRepository = facultyRepository;
            _programRepository = programRepository;
            _studentStatusRepository = studentStatusRepository;
        }


        public async Task<(bool Result, string ErrorCode)> StudentChecked(string propertyName, StudentDTO student, bool isUpdate = false)
        {
            switch (propertyName)
            {
                case nameof(StudentDTO.Email):
                    return student.Email is null || await CheckEmailAsync(student.Email)
                        ? (true, string.Empty)
                        : (false, "DUPLICATE_EMAIL");

                case nameof(StudentDTO.Phone):
                    return student.Phone is null || await CheckPhoneAsync(student.Phone)
                        ? (true, string.Empty)
                        : (false, "DUPLICATE_PHONE");

                case nameof(StudentDTO.Faculty):
                    return student.Faculty is null || await CheckFacultyAsync(student.Faculty)
                        ? (true, string.Empty)
                        : (false, "INVALID_FACULTY");

                case nameof(StudentDTO.Program):
                    return student.Program is null || await CheckProgramAsync(student.Program)
                        ? (true, string.Empty)
                        : (false, "INVALID_PROGRAM");

                case nameof(StudentDTO.Status):
                    if (isUpdate)
                        return student.Status is null || student.Id is null || await CheckUpdateStatusAsync(student.Id, student.Status)
                            ? (true, string.Empty)
                            : (false, "INVALID_STATUS_UPDATE");
                    return student.Status is null || await CheckStatusAsync(student.Status)
                        ? (true, string.Empty)
                        : (false, "INVALID_STATUS");

                case nameof(StudentDTO.Identity):
                    return student.Identity?.DocumentNumber is null || await CheckDocumentNumberAsync(student.Identity.DocumentNumber)
                        ? (true, string.Empty)
                        : (false, "DUPLICATE_IDENTITY");

                default:
                    return (true, string.Empty);
            }
        }


        private async Task<bool> CheckUpdateStatusAsync(string id, string status)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            var newStatus = await _studentStatusRepository.GetStudentStatusByIdAsync(status);
            if (student is null || newStatus is null)
                return false;
            return newStatus.Order >= student.Status.Order;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return !(await _studentRepository.IsEmailDuplicateAsync(email));
        }

        public async Task<bool> CheckPhoneAsync(string phone)
        {
            return !(await _studentRepository.IsPhoneDuplicateAsync(phone));
        }

        public async Task<bool> CheckDocumentNumberAsync(string documentNumber)
        {
            return !(await _studentRepository.IsDocumentNumberDuplicateAsync(documentNumber));
        }

        public async Task<bool> CheckFacultyAsync(string faculty)
        {
            return await _facultyRepository.GetFacultyByIdAsync(faculty) is not null;
        }

        public async Task<bool> CheckProgramAsync(string program)
        {
            return await _programRepository.GetProgramByIdAsync(program) is not null;
        }

        public async Task<bool> CheckStatusAsync(string status)
        {
            return await _studentStatusRepository.GetStudentStatusByIdAsync(status) is not null;
        }
    }
}
