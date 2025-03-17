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
    /// Represents an address.
    /// </summary>
    [Table("address")]
    public class Address
    {
        /// <summary>
        /// Gets or sets the unique identifier for the address.
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ward of the address.
        /// </summary>
        [Column("ward", TypeName = "nvarchar(50)")]
        public string? Ward { get; set; }

        /// <summary>
        /// Gets or sets the district of the address.
        /// </summary>
        [Column("district", TypeName = "nvarchar(50)")]
        public string? District { get; set; }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        [Column("city", TypeName = "nvarchar(50)")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        [Column("country", TypeName = "nvarchar(50)")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the street of the address.
        /// </summary>
        [Column("street", TypeName = "nvarchar(50)")]
        public string? Street { get; set; }

        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property
        /// </summary>
        public Student Student { get; set; } = null!;
    }
}
