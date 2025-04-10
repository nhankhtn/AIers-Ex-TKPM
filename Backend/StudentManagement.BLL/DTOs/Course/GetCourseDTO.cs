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
    public class GetCourseDTO
    {
        
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
        public Guid FacultyId { get; set; }
        public string FacultyName { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string? RequiredCourseName { get; set; }
        public int? RequiredCourseId { get; set; }   
        public DateTime? DeletedAt { get; set; }     
        public DateTime CreatedAt { get; set; }

    }

    public class GetAllCoursesDTO 
    {
        public List<GetCourseDTO> courses { get; set; } = new List<GetCourseDTO>();
        public int Total { get; set; } = 0;
    }

}
