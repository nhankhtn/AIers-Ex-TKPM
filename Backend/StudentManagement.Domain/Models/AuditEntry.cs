using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    [Table("audit_entries")]
    public class AuditEntry
    {
        [Key]
        [Column("id",TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("meta_data", TypeName = "nvarchar(max)")]
        public string Metadata { get; set; } = string.Empty;

        [Required]
        [Column("start_time_utc", TypeName = "datetime2(0)")]
        public DateTime StartTimeUtc { get; set; }

        [Required]
        [Column("end_time_utc", TypeName = "datetime2(0)")]
        public DateTime EndTimeUtc { get; set; }

        [Required]
        [Column("is_success", TypeName ="bit")]
        public bool IsSuccess { get; set; }

        [Required]
        [Column("error_message", TypeName = "nvarchar(max)")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
