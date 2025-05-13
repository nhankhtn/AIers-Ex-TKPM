using StudentManagement.BLL.DTOs.Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Course
{
    public class UpdateCourseDTO
    {
        public LocalizedName CourseName { get; set; } = new();
        public int Credits { get; set; }
        public Guid FacultyId { get; set; }
        public LocalizedName Description { get; set; } = new();
    }
}