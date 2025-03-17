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
    [Index(nameof(Code), IsUnique = true)]
    [Table("program")]
    public class Program
    {
        /// <summary>
        /// Gets or sets the unique identifier for the program.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the code for the program.
        /// </summary>
        [Column("code", TypeName = "varchar(10)")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the program.
        /// </summary>
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the program was created.
        /// </summary>
        [Column("created_at", TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the program was last updated.
        /// </summary>
        [Column("updated_time", TypeName = "date")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of students associated with this program.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
