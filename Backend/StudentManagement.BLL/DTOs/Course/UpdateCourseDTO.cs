using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Course
{
    public class UpdateCourseDTO
    {
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
        public Guid FacultyId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}