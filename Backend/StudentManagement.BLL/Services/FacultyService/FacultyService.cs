using AutoMapper;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Utils;
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

        public async Task<Result<FacultyDTO>> AddFacultyAsync(string name)
        {
            var res = await _facultyRepository.AddFacultyAsync(new Domain.Models.Faculty { Name = name });
            if (res) return Result<FacultyDTO>.Fail("ADD_FACULTY_FAILED");

            return Result<FacultyDTO>.Ok(new FacultyDTO { Name = name });
        }

        public async Task<Result<FacultyDTO>> ChangeFacultyNameAsync(int id, string newName)
        {
            var res = await _facultyRepository.UpdateFacultyAsync(new Domain.Models.Faculty { Id = id, Name = newName });
            if (res) return Result<FacultyDTO>.Fail("CHANGE_FACULTY_NAME_FAILED");

            return Result<FacultyDTO>.Ok(new FacultyDTO { Id = id, Name = newName });
        }

        public async Task<Result<IEnumerable<FacultyDTO>>> GetAllFacultiesAsync()
        {
            var res = await _facultyRepository.GetAllFacultiesAsync();
            return Result<IEnumerable<FacultyDTO>>.Ok(_mapper.Map<IEnumerable<FacultyDTO>>(res));
        }
    }
}
