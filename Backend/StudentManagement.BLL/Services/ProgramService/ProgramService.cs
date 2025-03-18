using AutoMapper;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
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

        public async Task<Result<ProgramDTO>> AddProgramAsync(ProgramDTO programDTO)
        {
            var res = await _programRepository.AddProgramAsync(_mapper.Map<Program>(programDTO));
            if (!res.Success) return Result<ProgramDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<ProgramDTO>.Ok(_mapper.Map<ProgramDTO>(res.Data));
        }

        public async Task<Result<ProgramDTO>> UpdateProgramAsync(string id, ProgramDTO programDTO)
        {
            programDTO.Id = id;
            var res = await _programRepository.UpdateProgramAsync(_mapper.Map<Program>(programDTO));
            if (!res.Success) return Result<ProgramDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<ProgramDTO>.Ok(_mapper.Map<ProgramDTO>(res.Data));
        }

        public async Task<Result<IEnumerable<ProgramDTO>>> GetAllProgramAsync()
        {
            var res = await _programRepository.GetAllProgramsAsync();
            if (!res.Success) return Result<IEnumerable<ProgramDTO>>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<IEnumerable<ProgramDTO>>.Ok(_mapper.Map<IEnumerable<ProgramDTO>>(res.Data));
        }
    }
}
