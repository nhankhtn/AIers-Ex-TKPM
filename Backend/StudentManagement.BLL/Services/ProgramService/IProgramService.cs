using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.Domain.Utils;
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

        Task<Result<ProgramDTO>> UpdateProgramAsync(string id, ProgramDTO programDTO);

        Task<Result<ProgramDTO?>> AddProgramAsync(ProgramDTO programDTO);

        Task<Result<string>> DeleteProgramAsync(string id);


    }
}
