using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Course
{
    public class AddCourseDTO
    {
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
        public Guid FacultyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? RequiredCourseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
