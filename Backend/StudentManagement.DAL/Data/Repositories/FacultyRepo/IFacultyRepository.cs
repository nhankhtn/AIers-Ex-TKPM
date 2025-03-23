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
        Task<Faculty?> GetFacultyByIdAsync(string id);

        Task<Faculty?> GetFacultyByIdAsync(Guid id);

        Task<Faculty?> GetFacultyByNameAsync(string name);

        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();

        Task<Faculty?> AddFacultyAsync(Faculty faculty);

        Task<Faculty?> UpdateFacultyAsync(Faculty faculty);

        Task DeleteFacultyAsync(Guid faculty);
    }
}
