using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhoneNumbers;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Validators
{
    public class StudentValidator : IStudentValidator
    {
        public IEmailService _emailService;

        public StudentValidator(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public List<string> NeedValidateProperties { get; } = new List<string>
        {
            nameof(StudentDTO.Course),
            nameof(StudentDTO.Email),
            nameof(StudentDTO.Phone)
        };

        public bool StudentValidate (string propertyName, StudentDTO student)
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


        private bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumber = phoneNumberUtil.Parse(phone, null);

            return phoneNumberUtil.IsValidNumber(phoneNumber);
        }
     
    }
}
