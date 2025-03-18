using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// Faculty's name
        /// </summary>
        [Required(ErrorMessage = "NAME_REQUIRED")]
        public string Name { get; set; } = string.Empty;
    }
}
