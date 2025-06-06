﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Class
{
    public class UpdateClassDTO
    {
        public string? ClassId { get; set; } = null!;

        public int? AcademicYear { get; set; }

        public string? CourseId { get; set; }

        public int? Semester { get; set; }

        public string? TeacherName { get; set; } = null!;

        public int? MaxStudents { get; set; }

        public string? Room { get; set; } = null!;

        public int? DayOfWeek { get; set; }

        public decimal? StartTime { get; set; }

        public decimal? EndTime { get; set; }

        public DateTime? Deadline { get; set; }
    }

}
