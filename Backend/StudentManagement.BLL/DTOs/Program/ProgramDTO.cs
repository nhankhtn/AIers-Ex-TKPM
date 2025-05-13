using StudentManagement.BLL.DTOs.Localize;
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
        public string? Id { get; set; }
        [Required(ErrorMessage = "NAME_REQUIRED")]
        public LocalizedName Name { get; set; } = new();
    }
}
