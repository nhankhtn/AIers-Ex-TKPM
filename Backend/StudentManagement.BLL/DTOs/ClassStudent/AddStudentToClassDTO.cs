using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{
    public class AddStudentToClassDTO
    {
        public int ClassId { get; set; }

        public string StudentId { get; set; } = null!;

        public decimal Score { get; set; } = 0;
    }
}
