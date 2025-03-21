using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Program
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
        /// Program's name
        /// </summary>
        [Required(ErrorMessage = "NAME_REQUIRED")]
        public string? Name { get; set; }
    }
}
