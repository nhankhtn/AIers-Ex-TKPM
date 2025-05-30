﻿using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.ClassStudent
{

    public class AddStudentToClassesDTO
    {
        public IEnumerable<string> ClassId { get; set; } = null!;
    }

    public class AddStudentToClassDTO
    {
        public string ClassId { get; set; } = null!;

        public string StudentId { get; set; } = null!;
    }
}
