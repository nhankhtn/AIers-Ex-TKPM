using StudentManagement.BLL.DTOs;
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

        Task<Result<ProgramDTO>> UpdateProgramAsync(int id, ProgramDTO programDTO);

        Task<Result<ProgramDTO>> AddProgramAsync(ProgramDTO programDTO);

    }
}
