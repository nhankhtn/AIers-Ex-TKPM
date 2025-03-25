using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.Domain.Attributes;
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
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    [Table("students")]
    public class Student
    {
        
        [Key]
        [Column("id", TypeName = "varchar(8)")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        
        [Required]
        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        
        [Required]
        [Column("gender", TypeName = "varchar(10)")]
        public Gender Gender { get; set; } = Gender.Male;

        [Required]
        [Column("email", TypeName = "varchar(50)")]
        public string Email { get; set; } = string.Empty;

        
        [Required]
        [Column("course", TypeName = "int")]
        public int Course { get; set; }

       
        [Required]
        [Column("phone", TypeName = "varchar(20)")]
        public string Phone { get; set; } = string.Empty;

       
        [Required]
        [Column("permanent_address")]
        public string PermanentAddress { get; set; } = string.Empty;

       
        [Column("temporary_address", TypeName = "nvarchar(max)")]
        public string? TemporaryAddress { get; set; }

       
        [Column("mailing_address", TypeName= "nvarchar(max)")]
        public string? MailingAddress { get; set; } = string.Empty;

        
        [Required]
        [Column("program_id", TypeName = "uniqueidentifier")]
        public Guid ProgramId { get; set; }

        
        [Required]
        [Column("status_id", TypeName = "uniqueidentifier")]
        public Guid StatusId { get; set; }

       
        [Required]
        [Column("faculty_id", TypeName = "uniqueidentifier")]
        public Guid FacultyId { get; set; }

        
        [Required]
        [Column("nationality", TypeName = "nvarchar(50)")]
        public string Nationality { get; set; } = string.Empty;

        public Faculty Faculty { get; set; } = null!;

        
        public Program Program { get; set; } = null!;

       
        public StudentStatus Status { get; set; } = null!;

       
        public Identity Identity { get; set; } = null!;
    }
}
