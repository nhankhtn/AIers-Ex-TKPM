using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    [Table("identity_documents")]
    [Index(nameof(DocumentNumber), IsUnique = true)]
    public class Identity
    {
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("type", TypeName = "varchar(10)")]
        public IdentityType Type { get; set; } 

        [Required]
        [Column("number", TypeName = "varchar(20)")] 
        public string DocumentNumber { get; set; } = string.Empty;
        
        [Required]
        [Column("issued_date", TypeName="date")]
        public DateTime IssueDate { get; set; }

        [Required]
        [Column("expiry_date", TypeName = "date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Column("issue_place", TypeName = "nvarchar(100)")]
        public string IssuePlace { get; set; } = string.Empty;

        [Column("country", TypeName = "nvarchar(50)")]
        public string? Country { get; set; }


        [Column("is_chip", TypeName = "bit")]
        public bool IsChip { get; set; } = false;

        [Column("note", TypeName = "nvarchar(100)")]
        public string? Notes { get; set; }


        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;
        public Student Student { get; set; } = null!;
    }
}
