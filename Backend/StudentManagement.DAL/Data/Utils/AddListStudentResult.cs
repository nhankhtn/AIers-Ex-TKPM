using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Utils
{
    public class AddListStudentResult
    {
        /// <summary>
        /// List of students that are added successfully
        /// </summary>
        public List<Student> AcceptableStudent { get; set; } = new List<Student>();

        /// <summary>
        /// List of students that are not added successfully
        /// </summary>
        public List<Student> UnacceptableStudent { get; set; } = new List<Student>();
    }
}
