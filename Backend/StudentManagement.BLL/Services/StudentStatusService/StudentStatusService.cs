using AutoMapper;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.StudentStatus;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
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

        public async Task<Result<StudentStatusDTO>> AddStudentStatusAsync(StudentStatusDTO studentStatusDTO)
        {
            var res = await _studentStatusRepository.AddStudentStatusAsync(_mapper.Map<StudentStatus>(studentStatusDTO));
            if (!res.Success) return Result<StudentStatusDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<StudentStatusDTO>.Ok(_mapper.Map<StudentStatusDTO>(res.Data), res.Message);
        }

        public async Task<Result<StudentStatusDTO>> UpdateStudentStatusAsync(string id, StudentStatusDTO studentStatusDTO)
        {
            studentStatusDTO.Id = id;
            var res = await _studentStatusRepository.UpdateStudentStatusAsync(_mapper.Map<StudentStatus>(studentStatusDTO));
            if (!res.Success) return Result<StudentStatusDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<StudentStatusDTO>.Ok(_mapper.Map<StudentStatusDTO>(res.Data), res.Message);
        }

        public async Task<Result<IEnumerable<StudentStatusDTO>>> GetAllStudentStatusAsync()
        {
            var res = await _studentStatusRepository.GetAllStudentStatusesAsync();
            if (!res.Success) return Result<IEnumerable<StudentStatusDTO>>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<IEnumerable<StudentStatusDTO>>.Ok(_mapper.Map<IEnumerable<StudentStatusDTO>>(res.Data), res.Message);
        }

        public async Task<Result<StudentStatusDTO>> DeleteStudentStatusAsync(string key)
        {
            var res = await _studentStatusRepository.DeleteStudentStatusAsync(new StudentStatus() { Id = key.ToGuid(), Name = key });
            if (!res.Success) return Result<StudentStatusDTO>.Fail(res.ErrorCode, res.ErrorMessage);
            return Result<StudentStatusDTO>.Ok(_mapper.Map<StudentStatusDTO>(res.Data), res.Message);
        }
    }
}
