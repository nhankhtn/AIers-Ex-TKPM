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
    [Index(nameof(NameEng), IsUnique = true)]

    [Table("programs")]
    public class Program
    {
        
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name", TypeName = "nvarchar(50)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("name_eng", TypeName = "nvarchar(50)")]
        public string NameEng { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
