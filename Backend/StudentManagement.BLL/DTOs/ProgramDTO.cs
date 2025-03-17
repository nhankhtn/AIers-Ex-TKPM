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
        public string? Id { get; set; }

        /// <summary>
        /// Program's code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Program's name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Program's Created time
        /// </summary>
        public DateTime? CreatedAt { get; set; }


        /// <summary>
        /// Program's Updated time
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
