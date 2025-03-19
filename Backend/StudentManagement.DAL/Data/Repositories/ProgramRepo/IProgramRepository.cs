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
        Task<Result<Program?>> GetProgramByIdAsync(string id);
        Task<Result<Program?>> GetProgramByIdAsync(Guid id);


        Task<Result<Program?>> GetProgramByNameAsync(string name);

        Task<Result<IEnumerable<Program>>> GetAllProgramsAsync();

        Task<Result<Program>> AddProgramAsync(Program program);

        Task<Result<Program>> UpdateProgramAsync(Program program);

        Task<Result<Program>> DeleteProgramAsync(Program program);

    }
}
