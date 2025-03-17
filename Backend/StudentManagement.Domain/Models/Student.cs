using Microsoft.EntityFrameworkCore;
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
    [Table("student")]
    public class Student
    {
        // Properties

        /// <summary>
        /// Gets or sets the student's unique identifier.
        /// </summary>
        [Key]
        [Column("id", TypeName = "varchar(8)")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the student's name.
        /// </summary>
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the student's date of birth.
        /// </summary>
        [Required]
        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the student's gender.
        /// </summary>
        [Column("gender", TypeName = "varchar(10)")]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Gets or sets the student's email.
        /// </summary>
        [Column("email", TypeName = "varchar(50)")]
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the student's course year.
        /// </summary>
        [Column("course", TypeName = "int")]
        public int? Course { get; set; }

        /// <summary>
        /// Gets or sets the student's phone number.
        /// </summary>
        [Required]
        [Column("phone", TypeName = "varchar(10)")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the student's temporary address.
        /// </summary>
        [Column("temporary_address", TypeName = "nvarchar(100)")]
        public string? TemporaryAddress { get; set; }

        /// <summary>
        /// Gets or sets the student's mailing address.
        /// </summary>
        [Column("mailing_address", TypeName="nvarchar(100)")]
        public string? MailingAddress { get; set; }

        /// <summary>
        /// Gets or sets the student's program ID.
        /// </summary>
        [Required]
        [Column("program_id", TypeName = "uniqueidentifier")]
        public Guid ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the student's status ID.
        /// </summary>
        [Required]
        [Column("status_id", TypeName = "uniqueidentifier")]
        public Guid StatusId { get; set; }

        /// <summary>
        /// Gets or sets the student's faculty ID.
        /// </summary>
        [Required]
        [Column("faculty_id", TypeName = "uniqueidentifier")]
        public Guid FacultyId { get; set; }

        // Navigation properties

        /// <summary>
        /// Gets or sets the faculty associated with the student.
        /// </summary>
        public Faculty Faculty { get; set; } = null!;

        /// <summary>
        /// Gets or sets the program associated with the student.
        /// </summary>
        public Program Program { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status associated with the student.
        /// </summary>
        public StudentStatus Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets the address associated with the student.
        /// </summary>
        public Address? PermanentAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identity associated with the student.
        /// </summary>
        public Identity? Identity { get; set; } = null!;

        /// <summary>
        /// Gets or sets the nationalities associated with the student.
        /// </summary>
        public StudentNationalities? Nationalities { get; set; } = null!;
    }
}
