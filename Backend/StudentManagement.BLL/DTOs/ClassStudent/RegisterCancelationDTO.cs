using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{
    public class RegisterCancelationDTO
    {
        public int ClassId { get; set; }

        public string StudentId { get; set; } = null!;
    }
}
