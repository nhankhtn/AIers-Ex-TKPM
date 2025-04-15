using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{
    public class GetRegisterCancelationHistoryDTO
    {
        public IEnumerable<RegisterCancelationHistoryRow> Data { get; set; } = null!;
        public int Total { get; set; }
    }

    public class RegisterCancelationHistoryRow
    {
        public string ClassId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public int Semester { get; set; }
        public string AcademicYear { get; set; } = null!;
        public DateTime Time { get; set; }
    }
}
