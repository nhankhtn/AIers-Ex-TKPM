﻿using StudentManagement.BLL.DTOs;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentStatusService
{
    public interface IStudentStatusService
    {
        Task<Result<IEnumerable<StudentStatusDTO>>> GetAllStudentStatusAsync();

        Task<Result<StudentStatusDTO>> UpdateStudentStatusAsync(string id, StudentStatusDTO studentStatusDTO);

        Task<Result<StudentStatusDTO>> AddStudentStatusAsync(StudentStatusDTO studentStatusDTO);
    }
}
