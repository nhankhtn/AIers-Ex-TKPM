using StudentManagement.BLL.DTOs.Localize;
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
        public string? Id { get; set; }

        public LocalizedName Name { get; set; } = new();

        public int? Order { get; set; }
    }
}
