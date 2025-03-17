using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace StudentManagement.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    public class Faculty
    {
        [Key]
        [Column("faculty_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("faculty_code", TypeName = "varchar(10)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("faculty_name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
