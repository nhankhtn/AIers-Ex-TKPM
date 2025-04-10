using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Class
{
    public class UpdateClassDTO
    {
        public int? AcademicYear { get; set; }

        public int? CourseId { get; set; }

        public int? Semester { get; set; }

        public string? TeacherName { get; set; } = string.Empty;

        public int? MaxStudents { get; set; }

        public string? Room { get; set; } = string.Empty;

        public int? DayOfWeek { get; set; }

        public decimal? StartTime { get; set; }

        public decimal? EndTime { get; set; }

        public DateTime? Deadline { get; set; }
    }

}
