using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Models
{
    [Table("classes")]
    public class Class
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("academic_year")]
        public int AcademicYear { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [Column("semester")]
        [Range(1, 3)]
        public int Semester { get; set; }

        [Column("teacher_name", TypeName = "nvarchar(50)")]
        public string TeacherName { get; set; } = null!;

        [Column("max_students")]
        public int MaxStudents { get; set; }

        [Column("room", TypeName = "varchar(10)")]
        public string Room { get; set; } = null!;

        [Column("day_of_week")]
        [Range(2, 8)]
        public int DayOfWeek { get; set; }

        [Column("start_time")]
        public decimal StartTime { get; set; }

        [Column("end_time")]
        public decimal EndTime { get; set; }

        [Column("deadline")]
        public DateTime Deadline { get; set; }

        public ICollection<ClassStudent> ClassStudents { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = null!;

    }
}
