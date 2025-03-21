using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.StudentStatus
{
    /// <summary>
    /// DTO for StudentStatus
    /// </summary>
    public class StudentStatusDTO
    {
        /// <summary>
        /// StudentStatus's id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// StudentStatus's name
        /// </summary>
        [Required(ErrorMessage = "NAME_REQUIRED")]
        public string? Name { get; set; }
    }
}
