﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.DTOs.Students
{
    public class AddListStudentDTO
    {
        public IEnumerable<StudentDTO> Students { get; set; } = new List<StudentDTO>();
    }
}
