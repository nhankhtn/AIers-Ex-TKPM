﻿using AutoMapper;
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
            catch (Exception ex)
            {
                return Result<ProgramDTO?>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<ProgramDTO>> UpdateProgramAsync(string id, ProgramDTO programDTO)
        {
            try
            {
                programDTO.Id = id;
                var studentStatus = _mapper.Map<StudentStatus>(programDTO);
                var existingStudentStatus = await _programRepository.GetProgramByIdAsync(id.ToGuid());
                if (existingStudentStatus == null)
                {
                    return Result<ProgramDTO>.Fail("404", "Student Status not found");
                }

                foreach (var prop in typeof(StudentStatus).GetProperties())
                {
                    var value = prop.GetValue(studentStatus);
                    if (value is null) continue;
                    if (prop.GetValue(existingStudentStatus) == value) continue;
                    prop.SetValue(existingStudentStatus, value);
                }

                var res = await _programRepository.UpdateProgramAsync(existingStudentStatus);
                return Result<ProgramDTO>.Ok(_mapper.Map<ProgramDTO>(res));
            }
            catch (Exception ex)
            {
                return Result<ProgramDTO>.Fail("500", ex.Message);
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
                return Result<IEnumerable<ProgramDTO>>.Fail("500", ex.Message);
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
                return Result<string>.Fail("500", ex.Message);
            }
        }

        
    }
}
