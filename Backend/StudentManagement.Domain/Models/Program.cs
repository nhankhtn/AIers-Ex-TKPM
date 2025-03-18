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
    /// <summary>
    /// Represents an academic program within the student management system.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    [Table("programs")]
    public class Program
    {
        /// <summary>
        /// Gets or sets the unique identifier for the program.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        

        /// <summary>
        /// Gets or sets the name of the program.
        /// </summary>
        [Column("name", TypeName = "nvarchar(50)")]
        [Required]
        public string Name { get; set; } = string.Empty;

       

        /// <summary>
        /// Gets or sets the collection of students associated with this program.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
