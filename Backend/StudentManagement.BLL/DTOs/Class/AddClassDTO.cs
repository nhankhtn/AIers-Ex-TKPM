using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Class
{
    public class AddClassDTO
    {
        public string ClassId { get; set; } = null!;

        public int AcademicYear { get; set; }

        public string CourseId { get; set; } = null!;

        public int Semester { get; set; }

        public string TeacherName { get; set; } = null!;

        public int MaxStudents { get; set; }

        public string Room { get; set; } = null!;

        public int DayOfWeek { get; set; }

        public decimal StartTime { get; set; }

        public decimal EndTime { get; set; }

        public DateTime Deadline { get; set; }
    }
}
