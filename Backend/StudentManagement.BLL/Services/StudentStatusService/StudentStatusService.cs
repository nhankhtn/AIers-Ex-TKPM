using AutoMapper;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentStatusService
{
    public class StudentStatusService : IStudentStatusService
    {
        private readonly IStudentStatusRepository _studentStatusRepository;
        private readonly IMapper _mapper;

        public StudentStatusService(IStudentStatusRepository studentStatusRepository, IMapper mapper)
        {
            _studentStatusRepository = studentStatusRepository;
            _mapper = mapper;
        }

        public async Task<Result<StudentStatusDTO>> AddStudentStatusAsync(string name)
        {
            var res = await _studentStatusRepository.AddStudentStatusAsync(new Domain.Models.StudentStatus { Name = name });
            if (res) return Result<StudentStatusDTO>.Fail("ADD_STUDENT_STATUS_FAILED");
            return Result<StudentStatusDTO>.Ok(new StudentStatusDTO { Name = name });
        }

        public async Task<Result<StudentStatusDTO>> ChangeStudentStatusNameAsync(int id, string newName)
        {
            var res = await _studentStatusRepository.UpdateStudentStatusAsync(new Domain.Models.StudentStatus { Id = id, Name = newName });
            if (res) return Result<StudentStatusDTO>.Fail("CHANGE_STUDENT_STATUS_NAME_FAILED");
            return Result<StudentStatusDTO>.Ok(new StudentStatusDTO { Id = id, Name = newName });
        }

        public async Task<Result<IEnumerable<StudentStatusDTO>>> GetAllStudentStatusAsync()
        {
            var res = await _studentStatusRepository.GetAllStudentStatusesAsync();
            return Result<IEnumerable<StudentStatusDTO>>.Ok(_mapper.Map<IEnumerable<StudentStatusDTO>>(res));
        }
    }
}
