using StudentManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    public class Identity
    {
        [Key]
        [Column("identity_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("identity_number", TypeName = "nvarchar(20)")]
        public IdentityType IdentityType { get; set; } // Loại giấy tờ
        
        [Required]
        [Column("issue_date", TypeName="date")]
        public DateTime IssueDate { get; set; } // Ngày cấp

        [Required]
        [Column("expiry_date", TypeName = "date")]
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        [Required]
        [Column("country", TypeName = "nvarchar(50)")]
        public string Country { get; set; } = string.Empty; // Nơi cấp 

        [Required]
        [Column("has_chip", TypeName = "bit")]
        public bool HasChip { get; set; } // Có chip hay không

        [Required]
        [Column("notes", TypeName = "nvarchar(100)")]
        public string? Notes { get; set; }

        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;

        public Student Student { get; set; } = new Student();
    }
}
