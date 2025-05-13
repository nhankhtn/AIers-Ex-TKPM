using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
   
    [Index(nameof(Name), IsUnique = true)]
    [Table("student_statuses")]
    public class StudentStatus
    {
        
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("name_eng", TypeName = "nvarchar(50)")]
        public string NameEng { get; set; } = string.Empty;


        [Required]
        [Column("order", TypeName = "int")]
        public int Order { get; set; } = 0;


        public ICollection<Student> Students { get; set; } = null!;
    }
}
