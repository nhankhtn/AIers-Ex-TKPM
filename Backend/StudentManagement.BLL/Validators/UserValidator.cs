using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhoneNumbers;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IStudentStatusRepository _studentStatusRepository;
        private readonly IEmailService _emailService;


        public UserValidator(IFacultyRepository facultyRepository, IProgramRepository programRepository, IStudentStatusRepository studentStatusRepository, IEmailService emailService)
        {
            _facultyRepository = facultyRepository;
            _programRepository = programRepository;
            _studentStatusRepository = studentStatusRepository;
            _emailService = emailService;
        }

        public List<string> NeedValidateProperties { get; } = new List<string>
        {
            nameof(StudentDTO.Course),
            nameof(StudentDTO.Email),
            nameof(StudentDTO.Phone),
            nameof(StudentDTO.Faculty),
            nameof(StudentDTO.Program),
            nameof(StudentDTO.Status)
        };

        public async Task<bool> StudentValidate (string propertyName, StudentDTO student)
        {
            switch (propertyName)
            {
                case nameof(StudentDTO.Course):
                    return student.Course is null || ValidateCourse((int)student.Course);

                // Validate email here
                case nameof(StudentDTO.Email):
                    return student.Email is null || ValidateEmail(student.Email);

                // Validate phone here
                case nameof(StudentDTO.Phone):
                    return student.Phone is null || ValidatePhone(student.Phone);


                case nameof(StudentDTO.Faculty):
                    return student.Faculty is null || await ValidateFaculty(student.Faculty);


                case nameof(StudentDTO.Program):
                    return student.Program is null || await ValidateProgram(student.Program);


                case nameof(StudentDTO.Status):
                    return student.Status is null || await ValidateStatus(student.Status);


                default: return true;
            }
        }


        private bool ValidateCourse (int course)
        {
            return course >= 2000;
        }

        private bool ValidateEmail (string email)
        {
            return _emailService.ValidateEmailAsync(email).Result;
        }

        private bool ValidatePhone (string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumber = phoneNumberUtil.Parse(phone, null);
            return phoneNumberUtil.IsValidNumber(phoneNumber);
        }

        private async Task<bool> ValidateFaculty (string faculty)
        {
            return await _facultyRepository.GetFacultyByIdAsync(faculty) != null;
        }

        private async Task<bool> ValidateProgram (string program)
        {
            return await _programRepository.GetProgramByIdAsync(program) != null;
        }

        private async Task<bool> ValidateStatus (string status)
        {
            return await _studentStatusRepository.GetStudentStatusByIdAsync(status) != null;
        }

    }
}
