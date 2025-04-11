using StudentManagement.BLL.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{
    public class GetClassStudentsDTO
    {
        public IEnumerable<GetClassStudentDTO> Data { get; set; } = null!;
        public int Total { get; set; } = 0;
    }
}
