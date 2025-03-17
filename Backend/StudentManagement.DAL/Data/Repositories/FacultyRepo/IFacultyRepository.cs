using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.FacultyRepo
{
    public interface IFacultyRepository
    {
        Task<Result<Faculty?>> GetFacultyByIdAsync(int id);

        Task<Result<Faculty?>> GetFacultyByNameAsync(string name);

        Task<Result<IEnumerable<Faculty>>> GetAllFacultiesAsync();

        Task<Result<Faculty>> AddFacultyAsync(Faculty faculty);

        Task<Result<Faculty>> UpdateFacultyAsync(Faculty faculty);
    }
}
