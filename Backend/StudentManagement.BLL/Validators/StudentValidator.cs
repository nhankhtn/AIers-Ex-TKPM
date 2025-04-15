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
using System.Runtime.CompilerServices;
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

        public (bool IsValid, string ErrorCode, string ErrorMessage) StudentValidate (StudentDTO student, bool isUpdate = false)
        {
            if ((!isUpdate || student.Course is not null) && !ValidateCourse(student.Course)) return (false, "INVALID_COURSE", "Khóa học không hợp lệ.");
            if ((!isUpdate || student.Email is not null) && !ValidateEmail(student.Email)) return (false, "INVALID_EMAIL", "Email không đúng định dạng.");
            if ((!isUpdate || student.Phone is not null) && !ValidatePhone(student.Phone)) return (false, "INVALID_PHONE", "Số điện thoại không đúng dịnh dạng.");
            return (true, string.Empty, string.Empty);
        }

        private bool ValidateCourse (int? course)
        {
            return course != null && course >= 2000;
        }

        private bool ValidateEmail (string? email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return _emailService.ValidateEmailAsync(email).Result;
        }

        private bool ValidatePhone(string? phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    return false;
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(phone, null);

                return phoneNumberUtil.IsValidNumber(phoneNumber);
            }
            catch (Exception)
            {
                return false;
            }
            
        }
     
    }
}
