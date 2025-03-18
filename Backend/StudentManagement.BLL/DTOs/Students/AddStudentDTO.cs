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
    public class AddStudentDTO
    {
        public string? Name { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public int? Gender { get; set; }

        public int? Course { get; set; }

        public string? Faculty { get; set; } = string.Empty;

        public string? Program { get; set; } = string.Empty;

        public string? Status { get; set; } = string.Empty;

        public string? Phone { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? TemporaryAddress { get; set; }

        public string? MailingAddress { get; set; } = string.Empty;

        public string? PermanentAddress { get; set; } = string.Empty;

        public string? Nationality { get; set; } = string.Empty;

        public IdentityDTO? Identity { get; set; }
    }
}
