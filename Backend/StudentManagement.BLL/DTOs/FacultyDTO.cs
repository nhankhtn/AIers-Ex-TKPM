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
        public string? Id { get; set; }

        /// <summary>
        /// Faculty's code
        /// </summary>
        public string? Code { get; set; } 

        /// <summary>
        /// Faculty's name
        /// </summary>
        public string? Name { get; set; } 

        /// <summary>
        /// Faculty's Created time
        /// </summary>
        public DateTime? CreatedAt { get; set; }


        /// <summary>
        /// Faculty's Updated time
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
