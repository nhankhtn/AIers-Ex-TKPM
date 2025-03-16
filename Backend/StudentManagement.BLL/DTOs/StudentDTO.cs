using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Enums;
using StudentManagement.Application.DTOs;

namespace StudentManagement.BLL.DTOs
{
    public class StudentDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? Gender { get; set; }

        public int? Faculty { get; set; }

        [RegularExpression(@"^20\d{2}$", ErrorMessage = "INVALID_COURSE")]
        public string? Course { get; set; }

        public int? ProgramId { get; set; }

        public int? StatusId { get; set; }

        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "INVALID_PHONE")]
        public string? Phone { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "INVALID_EMAIL")]
        public string? Email { get; set; }

        public AddressDTO? Address { get; set; }
    }
}
