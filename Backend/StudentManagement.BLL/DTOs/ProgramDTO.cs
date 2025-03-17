using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs
{
    /// <summary>
    /// DTO for Program
    /// </summary>
    public class ProgramDTO
    {
        /// <summary>
        /// Program's id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Program's code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Program's name
        /// </summary>
        public string? Name { get; set; }
    }
}
