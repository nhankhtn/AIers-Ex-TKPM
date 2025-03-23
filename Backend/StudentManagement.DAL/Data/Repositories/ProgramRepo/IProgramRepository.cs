using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ProgramRepo
{
    public interface IProgramRepository
    {
        Task<Program?> GetProgramByIdAsync(string id);
        Task<Program?> GetProgramByIdAsync(Guid id);
        Task<Program?> GetProgramByNameAsync(string name);

        Task<IEnumerable<Program>> GetAllProgramsAsync();

        Task<Program?> AddProgramAsync(Program program);

        Task<Program?> UpdateProgramAsync(Program program);

        Task DeleteProgramAsync(Guid programId);

    }
}
