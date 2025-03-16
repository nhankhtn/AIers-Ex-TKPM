using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ProgramRepo
{
    public interface IProgramRepository
    {
        Task<Program?> GetProgramByIdAsync(int id);

        Task<Program?> GetProgramByNameAsync(string name);

        Task<IEnumerable<Program>> GetAllProgramsAsync();

        Task<bool> AddProgramAsync(Program program);

        Task<bool> UpdateProgramAsync(Program program);
    }
}
