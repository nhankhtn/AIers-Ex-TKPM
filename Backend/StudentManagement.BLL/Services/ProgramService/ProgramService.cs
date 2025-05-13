using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.StudentStatus;
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

        public async Task<Result<ProgramDTO?>> AddProgramAsync(ProgramDTO programDTO)
        {
            try
            {
                var program = _mapper.Map<Program>(programDTO);
                var p = await _programRepository.AddProgramAsync(program);
                return Result<ProgramDTO?>.Ok(_mapper.Map<ProgramDTO>(p));
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_programs_name"))
            {
                return Result<ProgramDTO?>.Fail("DUPLICATE_PROGRAM_NAME", "Chương trình đã tồn tại.");
            }
            catch (Exception ex)
            {
                return Result<ProgramDTO?>.Fail("ADD_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<ProgramDTO>> UpdateProgramAsync(string id, ProgramDTO programDTO)
        {
            try
            {
                programDTO.Id = id;
                var program = _mapper.Map<Program>(programDTO);
                var existingProgram = await _programRepository.GetProgramByIdAsync(id.ToGuid());
                if (existingProgram == null)
                {
                    return Result<ProgramDTO>.Fail("UPDATE_PROGRAM_FAILED", "Chương trình không tồn tại.");
                }

                foreach (var prop in typeof(Program).GetProperties())
                {
                    var value = prop.GetValue(program);
                    if (value is null) continue;
                    if (prop.GetValue(existingProgram) == value) continue;
                    prop.SetValue(existingProgram, value);
                }

                var res = await _programRepository.UpdateProgramAsync(existingProgram);
                return Result<ProgramDTO>.Ok(_mapper.Map<ProgramDTO>(res));
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_programs_name"))
            {
                return Result<ProgramDTO>.Fail("DUPLICATE_PROGRAM_NAME", "Chương trình đã tồn tại.");
            }
            catch (Exception ex)
            {
                return Result<ProgramDTO>.Fail("UPDATE_PROGRAM_FAILED", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ProgramDTO>>> GetAllProgramAsync()
        {
            try
            {
                var programs = await _programRepository.GetAllProgramsAsync();
                return Result<IEnumerable<ProgramDTO>>.Ok(_mapper.Map<IEnumerable<ProgramDTO>>(programs));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProgramDTO>>.Fail("GET_PROGRAMS_FAILED", ex.Message);
            }
        }

        public async Task<Result<string>> DeleteProgramAsync(string id)
        {
            try
            {
                await _programRepository.DeleteProgramAsync(id.ToGuid());
                return Result<string>.Ok(id);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("DELETE_PROGRAM_FAILED", ex.Message);
            }
        }
    }
}
