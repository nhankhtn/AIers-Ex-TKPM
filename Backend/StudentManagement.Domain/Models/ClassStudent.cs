using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    [Table("class_student")]
    [PrimaryKey(nameof(ClassId), nameof(StudentId))]
    public class ClassStudent
    {
        [Column("class_id")]
        public string ClassId { get; set; } = null!;
        public Class Class { get; set; } = null!;

        [Column("student_id")]
        public string StudentId { get; set; } = null!;
        public Student Student { get; set; } = null!;

        [Column("score")]
        public decimal Score { get; set; }

        public RegisterCancellationHistory? RegisterCancellationHistories { get; set; } 
    }

    [Table("register_cancellation_history")]
    public class RegisterCancellationHistory
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("class_id")]
        public string? ClassId { get; set; }

        [Column("student_id")]
        public string? StudentId { get; set; }
        public DateTime Time { get; set; }
        public ClassStudent? ClassStudent { get; set; } = null!;
    }
}
