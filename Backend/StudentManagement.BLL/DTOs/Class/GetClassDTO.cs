using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Class
{
    public class GetClassDTO
    {
        public int? Id { get; set; }
        public int? AcademicYear { get; set; }
        public string? CourseId { get; set; }
        public int? Semester { get; set; }

        public string? TeacherName { get; set; } = string.Empty;

        public int? MaxStudents { get; set; }

        public string? Room { get; set; } = string.Empty;

        public int? DayOfWeek { get; set; }

        public decimal? StartTime { get; set; }

        public decimal? EndTime { get; set; }

        public DateTime? Deadline { get; set; }
    }


    public class GetClassesDTO
    {
        public IEnumerable<GetClassDTO> Data { get; set; } = new List<GetClassDTO>();
        public int Total { get; set; } = 0;
    }

}
