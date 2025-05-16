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
        public double MidTermScore { get; set; }

        [Column("final_score")]
        public double FinalScore { get; set; }

        [Column("GPA")]
        public double TotalScore { get; set; }

        [Column("grade")]
        public char Grade { get; set; }

        [Column("is_passed")]
        public bool IsPassed { get; set; }
    }

    [Table("register_cancellation_history")]
    public class RegisterCancellationHistory
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("class_id")]
        public string ClassId { get; set; } = null!;

        [Column("course_name")]
        public string CourseName { get; set; } = null!;

        [Column("course_name_eng")]
        public string CourseNameEng { get; set; } = null!;

        [Column("student_id")]
        public string StudentId { get; set; } = null!;

        [Column("student_name")]
        public string StudentName { get; set; } = null!;

        [Column("semester")]
        public int Semester { get; set; }

        [Column("academic_year")]
        public int AcademicYear { get; set; }

        public DateTime Time { get; set; }
    }
}
