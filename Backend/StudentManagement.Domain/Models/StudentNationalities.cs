using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class StudentNationalities
    {
        [Key]
        [Column("id", TypeName= "uniqueidentifier")]
        public Guid Id { get; set; }

        [Required]
        [Column("country", TypeName = "nvarchar(20)")]
        public string Country { get; set; } = string.Empty;

        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;

        
        public Student Student { get; set; } = null!;
    }
}
