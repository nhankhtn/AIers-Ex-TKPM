using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class Program
    {
        [Key]
        [Column("program_id", TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column("program_name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
