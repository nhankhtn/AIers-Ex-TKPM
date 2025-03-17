using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs
{
    /// <summary>
    /// DTO for StudentStatus
    /// </summary>
    public class StudentStatusDTO
    {
        /// <summary>
        /// StudentStatus's id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// StudentStatus's code
        /// </summary>
        public string? Code { get; set; }
        
        /// <summary>
        /// StudentStatus's name
        /// </summary>
        public string? Name { get; set; }
    }
}
