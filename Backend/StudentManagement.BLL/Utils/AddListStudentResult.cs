using StudentManagement.BLL.DTOs.Students;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Utils
{
    public class AddListStudentResult
    {
        /// <summary>
        /// List of students that are added successfully
        /// </summary>
        public List<StudentDTO> AcceptableStudent { get; set; } = new List<StudentDTO>();

        /// <summary>
        /// List of students that are not added successfully
        /// </summary>
        public List<StudentDTO> UnacceptableStudent { get; set; } = new List<StudentDTO>();
    }
}
