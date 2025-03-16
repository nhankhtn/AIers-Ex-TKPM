using AutoMapper;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ProgramService
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;

        public ProgramService(IProgramRepository programRepository, IMapper mapper)
        {
            _programRepository = programRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProgramDTO>> AddProgramAsync(string name)
        {
            var res = await _programRepository.AddProgramAsync(new Domain.Models.Program { Name = name });
            if (res) return Result<ProgramDTO>.Fail("ADD_PROGRAM_FAILED");
            return Result<ProgramDTO>.Ok(new ProgramDTO { Name = name });
        }

        public async Task<Result<ProgramDTO>> ChangeProgramNameAsync(int id, string newName)
        {
            var res = await _programRepository.UpdateProgramAsync(new Domain.Models.Program { Id = id, Name = newName });
            if (res) return Result<ProgramDTO>.Fail("CHANGE_PROGRAM_NAME_FAILED");
            return Result<ProgramDTO>.Ok(new ProgramDTO { Id = id, Name = newName });
        }

        public async Task<Result<IEnumerable<ProgramDTO>>> GetAllProgramAsync()
        {
            var res = await _programRepository.GetAllProgramsAsync();
            return Result<IEnumerable<ProgramDTO>>.Ok(_mapper.Map<IEnumerable<ProgramDTO>>(res));
        }
    }
}
