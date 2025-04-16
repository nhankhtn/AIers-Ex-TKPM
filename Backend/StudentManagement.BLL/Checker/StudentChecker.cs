using Microsoft.IdentityModel.Tokens;
using StudentManagement.BLL.Checker;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Validators
{
    public class StudentChecker : IStudentChecker
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IStudentStatusRepository _studentStatusRepository;

        public StudentChecker(IStudentRepository studentRepository, IFacultyRepository facultyRepository, IProgramRepository programRepository, IStudentStatusRepository studentStatusRepository)
        {
            _studentRepository = studentRepository;
            _facultyRepository = facultyRepository;
            _programRepository = programRepository;
            _studentStatusRepository = studentStatusRepository;
        }

        public async Task<(bool IsValid, string ErrorCode, string ErrorMessage)> StudentCheckAsync(StudentDTO studentDTO, bool isUpdate = false)
        {
            if ((!isUpdate || studentDTO.Email is not null) && !await CheckEmailAsync(studentDTO.Email, studentDTO.Id))
                return (false, "DUPLICATE_EMAIL", "Email đã tồn tại.");

            if ((!isUpdate || studentDTO.Phone is not null) && !await CheckPhoneAsync(studentDTO.Phone, studentDTO.Id))
                return (false, "DUPLICATE_PHONE", "Số điện thoại đã tồn tại");

            if ((!isUpdate || studentDTO.Faculty is not null) && !await CheckFacultyAsync(studentDTO.Faculty))
                return (false, "INVALID_FACULTY", "Mã khoa không hợp lệ.");

            if ((!isUpdate || studentDTO.Program is not null) && !await CheckProgramAsync(studentDTO.Program))
                return (false, "INVALID_PROGRAM", "Mã chương trình không hợp lệ.");

            if ((!isUpdate || studentDTO.Status is not null))
            {
                var res = await CheckStatusAsync(studentDTO.Status, studentDTO.Id);
                if (res == 0)
                    return (false, "INVALID_STATUS", "Mã trạng thái không hợp lệ.");
                else if (res == 1)
                {
                    if (studentDTO.Status != null)
                    {
                        var status = await _studentStatusRepository.GetStudentStatusByIdAsync(studentDTO.Status);
                        return (false, "INVALID_STATUS", $"Không thể quay lại trạng thái \"{status?.Name}\"");
                    }
                    return (false, "INVALID_STATUS", "Không thể quay lại trạng thái này.");
                }
            }    

            if ((!isUpdate || studentDTO.Identity?.DocumentNumber is not null) && !await CheckDocumentNumberAsync(studentDTO.Identity?.DocumentNumber, studentDTO.Id))
                return (false, "DUPLICATE_DOCUMENT_NUMBER", "Số giấy tờ đã tồn tại.");

            return (true, string.Empty, string.Empty);
        }

        private async Task<int> CheckStatusAsync(string? status, string? id)
        {
            if (status is null) return 0;
            var _status = await _studentStatusRepository.GetStudentStatusByIdAsync(status);
            if (_status == null) return 0;

            if (id is not null)
            {
                var student = await _studentRepository.GetStudentByIdAsync(id);
                if (student is null) return 0;
                return student.Status.Order <= _status.Order ? 2 : 1;
            }
            return 2;
        }

        private async Task<bool> CheckEmailAsync(string? email, string? userId = null)
        {
            if (string.IsNullOrEmpty(email)) return true;
            var std = await _studentRepository.GetStudentByEmailAsync(email);
            if (std is null) return true;
            return userId is not null && std.Id == userId;
        }

        private async Task<bool> CheckPhoneAsync(string? phone, string? userId = null)
        {
            if (string.IsNullOrEmpty(phone)) return true;
            var std = await _studentRepository.GetStudentByPhoneAsync(phone);
            if (std is null) return true;
            return userId is not null && std.Id == userId;
        }

        private async Task<bool> CheckDocumentNumberAsync(string? documentNumber, string? userId = null)
        {
            if (string.IsNullOrEmpty(documentNumber)) return true;
            var std = await _studentRepository.GetStudentByDocumentNumberAsync(documentNumber);
            if (std is null) return true;
            return userId is not null && std.Id == userId;
        }

        private async Task<bool> CheckFacultyAsync(string? faculty)
        {
            if (faculty is null) return true;
            return await _facultyRepository.GetFacultyByIdAsync(faculty) is not null;
        }

        private async Task<bool> CheckProgramAsync(string? program)
        {
            if (program is null) return true;
            return await _programRepository.GetProgramByIdAsync(program) is not null;
        }
    }
}
