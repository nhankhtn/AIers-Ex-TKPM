using AutoMapper;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.FacultyService
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IMapper _mapper;

        public FacultyService(IFacultyRepository facultyRepository, IMapper mapper)
        {
            _facultyRepository = facultyRepository;
            _mapper = mapper;
        }

        public async Task<Result<FacultyDTO>> AddFacultyAsync(FacultyDTO facultyDTO)
        {
            var res = await _facultyRepository.AddFacultyAsync(_mapper.Map<Faculty>(facultyDTO));
            if (!res.Success) return Result<FacultyDTO>.Fail(res.ErrorCode, res.ErrorMessage);

            return Result<FacultyDTO>.Ok(_mapper.Map<FacultyDTO>(res.Data));
        }

        public async Task<Result<FacultyDTO>> UpdateFacultyAsync(string id, FacultyDTO facultyDTO)
        {
            facultyDTO.Id = id;
            var res = await _facultyRepository.UpdateFacultyAsync(_mapper.Map<Faculty>(facultyDTO));
            if (!res.Success) return Result<FacultyDTO>.Fail(res.ErrorCode, res.ErrorMessage);

            return Result<FacultyDTO>.Ok(_mapper.Map<FacultyDTO>(res.Data));
        }

        public async Task<Result<IEnumerable<FacultyDTO>>> GetAllFacultiesAsync()
        {
            var res = await _facultyRepository.GetAllFacultiesAsync();
            if (!res.Success) return Result<IEnumerable<FacultyDTO>>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<IEnumerable<FacultyDTO>>.Ok(_mapper.Map<IEnumerable<FacultyDTO>>(res.Data));
        }
    }
}
