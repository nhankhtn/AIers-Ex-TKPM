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
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    [Table("faculty")]
    public class Faculty
    {
        /// <summary>
        /// Gets or sets the unique identifier for the faculty.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the code for the faculty.
        /// </summary>
        [Column("code", TypeName = "varchar(10)")]
        public string? Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the faculty.
        /// </summary>
        [Column("name", TypeName = "nvarchar(50)")]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the faculty was created.
        /// </summary>
        [Column("created_at", TypeName = "date")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the faculty was last updated.
        /// </summary>
        [Column("updated_at", TypeName = "date")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of students associated with this faculty.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
