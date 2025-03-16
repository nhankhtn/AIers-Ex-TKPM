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
        [Column("gender", TypeName = "nvarchar(20)")]
        public Gender Gender { get; set; } 

        [Required]
        [Column("course", TypeName = "varchar(10)")]
        public string? Course{ get; set; }

        [Required]
        [Column("phone", TypeName = "varchar(10)")]
        public string? Phone { get; set; }

        [Required]
        [Column("email", TypeName = "varchar(50)")]
        public string? Email { get; set; }

        // Foreign key

        [Required]
        [Column("program_id", TypeName = "int")]
        public int ProgramId { get; set; }

        [Required]
        [Column("status_id", TypeName = "int")]
        public int StatusId { get; set; }

        [Required]
        [Column("address_id", TypeName = "int")]
        public int AddressId { get; set; }

        [Required]
        [Column("faculty_id", TypeName = "int")]
        public int FacultyId { get; set; } 

        [Required]
        [Column("identity_id", TypeName = "int")]
        public int IdentityId { get; set; }

        // Navigation properties

        public Faculty Faculty { get; set; } = null!;

        public Program Program { get; set; } = null!;

        public StudentStatus Status { get; set; } = null!;

        public Address? Address { get; set; } = null!;

        public Identity? Identity { get; set; } = null!;

    }
}