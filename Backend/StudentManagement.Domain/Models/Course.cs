﻿using Microsoft.EntityFrameworkCore;
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
        [Column("id")]
        [Key]
        public int CourseId { get; set; }

        [Column("name", TypeName ="nvarchar(50)")]
        public string CourseName { get; set; } = null!;

        [Column("credits")]
        [Range(2, int.MaxValue)]
        public int Credits { get; set; }

        [Column("faculty_id")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; } = null!;

        [Column("description", TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
        public Course? RequiredCourse { get; set; }

        [Column("required_course_id")]
        public int? RequiredCourseId { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        public ICollection<Class> Classes { get; set; } = null!;
    }
}
