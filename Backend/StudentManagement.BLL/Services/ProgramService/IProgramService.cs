using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ProgramService
{
    public interface IProgramService
    {
        Task<Result<IEnumerable<ProgramDTO>>> GetAllProgramAsync();

        Task<Result<ProgramDTO>> ChangeProgramNameAsync(int id, string newName);

        Task<Result<ProgramDTO>> AddProgramAsync(string name);

    }
}
