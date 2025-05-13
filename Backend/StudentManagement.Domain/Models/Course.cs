using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    [Table("courses")]
    [Index(nameof(CourseName), IsUnique = true)]
    public class Course
    {
        [Column("id", TypeName ="varchar(10)")]
        [Key]
        public string CourseId { get; set; } = null!;

        [Column("name", TypeName ="nvarchar(50)")]
        public string CourseName { get; set; } = null!;

        [Column("name_eng", TypeName = "nvarchar(50)")]
        public string CourseNameEng { get; set; } = null!;

        [Column("credits")]
        [Range(2, int.MaxValue)]
        public int Credits { get; set; }

        [Column("faculty_id")]
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; } = null!;

        [Column("description", TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Column("description_eng", TypeName = "nvarchar(max)")]
        public string DescriptionEng { get; set; } = string.Empty;
        public Course? RequiredCourse { get; set; }

        [Column("required_course_id")]
        public string? RequiredCourseId { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public ICollection<Class> Classes { get; set; } = null!;
    }
}
