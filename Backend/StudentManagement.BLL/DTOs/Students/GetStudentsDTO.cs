using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Students
{
    /// <summary>
    /// Data transfer object for getting students
    /// </summary>
    public class GetStudentsDTO
    {
        /// <summary>
        /// List of students
        /// </summary>
        public IEnumerable<StudentDTO> Data { get; set; } = new List<StudentDTO>();
        public int Total { get; set; }
    }
}