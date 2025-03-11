using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class Student
    {
        [Key]
        [Column("student_id", TypeName = "varchar(8)")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("student_name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column("gender")]
        public bool Gender { get; set; } // true: male, false: female

        [Required]
        [Column("faculty")]
        public Faculty Faculty { get; set; }

        [Required]
        [Column("academic_year", TypeName = "varchar(10)")]
        public string? AcademicYear { get; set; }

        [Required]
        [Column("program", TypeName = "nvarchar(50)")]
        public string? Program { get; set; }

        [Required]
        [Column("status")]
        public StudentStatus Status { get; set; }

        [Required]
        [Column("phone", TypeName = "varchar(10)")]
        public string? Phone { get; set; }

        [Required]
        [Column("email", TypeName = "varchar(50)")]
        public string? Email { get; set; }

        [Required]
        [Column("address", TypeName = "nvarchar(100)")]
        public string? Address { get; set; }
    }
}