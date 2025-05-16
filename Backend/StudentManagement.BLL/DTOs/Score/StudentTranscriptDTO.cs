using StudentManagement.BLL.DTOs.Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Score
{
    public class StudentTranscriptDTO
    {
        public IEnumerable<TranscriptRow> Transcript { get; set; } = null!;
        public int TotalCredit { get; set; }
        public int PassedCredit { get; set; }
        public double GPA { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public LocalizedName FacultyName { get; set; } = null!;
        public int Course { get; set; }
    }

    public class TranscriptRow
    {
        public string ClassId { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public LocalizedName CourseName { get; set; } = null!;
        public int Credit { get; set; } = 0;
        public double TotalScore { get; set; } = 0;
        public char Grade { get; set; }
    }
}
