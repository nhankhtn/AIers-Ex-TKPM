using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.BLL.DTOs.Localize;

namespace StudentManagement.BLL.DTOs.Course
{
    public class AddCourseDTO
    {
        public string CourseId { get; set; } = null!;
        public LocalizedName CourseName { get; set; } = new();
        public int Credits { get; set; }
        public Guid FacultyId { get; set; }
        public LocalizedName Description { get; set; } = new();
        public string? RequiredCourseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
