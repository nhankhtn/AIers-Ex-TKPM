﻿using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassRepo
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetClassesAsync(string? classId = null, int? semester = null, int? year = null, int? page = null, int? limit = null);
        Task<Class?> GetClassByIdAsync(string id);
        Task<Class?> AddClassAsync(Class classEntity);
        Task UpdateClassAsync(Class classEntity);
        Task DeleteClassAsync(string id);
        Task<Course?> GetCourseByClassAsync(string classId);
        Task<int> GetCreditsAsync(string classId);
        Task<(string vi, string en)> GetCourseNameAsync(string classId);
    }
}
