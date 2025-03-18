using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Enums;
using StudentManagement.Application.DTOs;

namespace StudentManagement.BLL.DTOs.Students
{
    public class StudentDTO
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "NAME_REQUIRED")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "DATE_OF_BIRTH_REQUIRED")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "GENDER_REQUIRED")]
        public int Gender { get; set; }

        public int? Course { get; set; }

        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "INVALID_PHONE")]
        [Required(ErrorMessage = "PHONE_REQUIRED")]
        public string Phone { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "INVALID_EMAIL")]
        public string? Email { get; set; }

        public string? TemporaryAddress { get; set; }

        [Required(ErrorMessage = "MAILING_ADDRESS_REQUIRED")]
        public string MailingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "PERMANENT_ADDRESS_REQUIRED")]
        public string PermanentAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "NATIONALITY_REQUIRED")]
        public string Nationality { get; set; } = string.Empty;

        public FacultyDTO? Faculty { get; set; }

        public ProgramDTO? Program { get; set; }

        public StudentStatusDTO? Status { get; set; }

        public IdentityDTO? Identity { get; set; }
    }
}
