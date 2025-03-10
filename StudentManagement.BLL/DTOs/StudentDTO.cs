using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs
{
    public class StudentDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Faculty { get; set; }


        public string? AcademicYear { get; set; }

        public string? Program { get; set; }


        public string? Status { get; set; }

        public string? Phone { get; set; }


        public string? Email { get; set; }

  
        public string? Address { get; set; }
    }
}
