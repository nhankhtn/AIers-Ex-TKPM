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
    [Table("identity_documents")]
    public class Identity
    {
        /// <summary>
        /// Identity's Id
        /// </summary>
        [Key]
        [Column("id", TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        /// <summary>
        /// Identity type
        /// </summary>
        [Required]
        [Column("type", TypeName = "varchar(10)")]
        public IdentityType Type { get; set; } = IdentityType.CCCD; // Loại giấy tờ

        /// <summary>
        /// Document's number
        /// </summary>
        [Required]
        [Column("document_number", TypeName = "varchar(20)")] 
        public string DocumentNumber { get; set; } = string.Empty; // Số của giấy tờ
        
        /// <summary>
        /// Issue date
        /// </summary>
        [Required]
        [Column("issue_date", TypeName="date")]
        public DateTime IssueDate { get; set; } // Ngày cấp

        /// <summary>
        /// Issue place
        /// </summary>
        [Required]
        [Column("issue_place", TypeName = "nvarchar(50)")]
        public string IssuePlace { get; set; } = string.Empty;

        /// <summary>
        /// Expiry date
        /// </summary>
        [Required]
        [Column("expiry_date", TypeName = "date")]
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        /// <summary>
        /// Country
        /// </summary>
        [Column("country", TypeName = "nvarchar(50)")]
        public string? Country { get; set; } // Nơi cấp 

        /// <summary>
        /// Document has a chip
        /// </summary>
        [Column("is_chip", TypeName = "bit")]
        public bool? IsChip { get; set; } // Có chip hay không


        /// <summary>
        /// Document's note
        /// </summary>
        [Column("notes", TypeName = "nvarchar(100)")]
        public string? Notes { get; set; }


        /// <summary>
        /// Foreign Key to student 
        /// </summary>
        [Required]
        [Column("student_id", TypeName = "varchar(8)")]
        public string StudentId { get; set; } = string.Empty;

        public Student Student { get; set; } = new Student();
    }
}
