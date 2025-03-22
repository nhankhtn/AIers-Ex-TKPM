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
    /// <summary>
    /// Represents a faculty within the student management system.
    /// </summary>
    [Table("faculty")]
    [Index(nameof(Name), IsUnique = true)]
    public class Faculty
    {
        /// <summary>
        /// Gets or sets the unique identifier for the faculty.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the faculty.
        /// </summary>
        [Column("name", TypeName = "nvarchar(50)")]
        [Required]
        public string Name { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the collection of students associated with this faculty.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
