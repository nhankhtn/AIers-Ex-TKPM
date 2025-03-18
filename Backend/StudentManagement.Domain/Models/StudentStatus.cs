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
    /// Represents the status of a student.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    [Table("status")]
    public class StudentStatus
    {
        /// <summary>
        /// Gets or sets the unique identifier for the student status.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the student status.
        /// </summary>
        [Required]
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of students associated with this status.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
