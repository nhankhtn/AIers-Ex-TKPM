﻿using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.FacultyRepo
{
    public interface IFacultyRepository
    {
        Task<Faculty?> GetFacultyByIdAsync(int id);

        Task<Faculty?> GetFacultyByNameAsync(string name);

        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();

        Task<bool> AddFacultyAsync(Faculty faculty);

        Task<bool> UpdateFacultyAsync(Faculty faculty);
    }
}
