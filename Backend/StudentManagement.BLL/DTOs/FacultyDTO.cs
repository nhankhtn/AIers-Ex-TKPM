using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs
{
    /// <summary>
    /// DTO for Faculty
    /// </summary>
    public class FacultyDTO
    {
        /// <summary>
        /// Faculty's id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Faculty's code
        /// </summary>
        public string? Code { get; set; } = string.Empty;

        /// <summary>
        /// Faculty's name
        /// </summary>
        public string? Name { get; set; } = string.Empty;
    }
}
