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
    
    [Table("faculties")]
    [Index(nameof(Name), IsUnique = true)]
    public class Faculty
    {
        
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

       
        [Column("name", TypeName = "nvarchar(50)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}
