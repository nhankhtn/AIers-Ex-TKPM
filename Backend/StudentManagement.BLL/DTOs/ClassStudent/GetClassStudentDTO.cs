using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{
    public class GetClassStudentDTO
    {
        public string ClassId { get; set; } = null!;

        public string StudentId { get; set; } = null!;

        public decimal Score { get; set; } = 0;
    }
}
