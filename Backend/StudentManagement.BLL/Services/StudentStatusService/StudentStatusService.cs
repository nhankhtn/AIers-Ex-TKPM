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
            try
            {
                var student = _mapper.Map<StudentStatus>(studentStatusDTO);
                var std = await _studentStatusRepository.AddStudentStatusAsync(student);
                return Result<StudentStatusDTO>.Ok(_mapper.Map<StudentStatusDTO>(std));
            }
            catch (Exception ex)
            {
                return Result<StudentStatusDTO>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<StudentStatusDTO>> UpdateStudentStatusAsync(string id, StudentStatusDTO studentStatusDTO)
        {
            try
            {
                studentStatusDTO.Id = id;
                var studentStatus = _mapper.Map<StudentStatus>(studentStatusDTO);
                var existingStudentStatus = await _studentStatusRepository.GetStudentStatusByIdAsync(id.ToGuid());
                if (existingStudentStatus == null)
                {
                    return Result<StudentStatusDTO>.Fail("404", "Student Status not found");
                }

                foreach (var prop in typeof(StudentStatus).GetProperties())
                {
                    var value = prop.GetValue(studentStatus);
                    if (value is null) continue;
                    if (prop.GetValue(existingStudentStatus) == value) continue;
                    prop.SetValue(existingStudentStatus, value);
                }

                var res = await _studentStatusRepository.UpdateStudentStatusAsync(existingStudentStatus);
                return Result<StudentStatusDTO>.Ok(_mapper.Map<StudentStatusDTO>(res));
            }
            catch (Exception ex)
            {
                return Result<StudentStatusDTO>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<StudentStatusDTO>>> GetAllStudentStatusAsync()
        {
            try
            {
                var studentStatuses =  await _studentStatusRepository.GetAllStudentStatusesAsync();
                return Result<IEnumerable<StudentStatusDTO>>.Ok(_mapper.Map<IEnumerable<StudentStatusDTO>>(studentStatuses));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<StudentStatusDTO>>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<string>> DeleteStudentStatusAsync(string id)
        {
            try
            {
                await _studentStatusRepository.DeleteStudentStatusAsync(id.ToGuid());
                return Result<string>.Ok(id);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("500", ex.Message);
            }
        }
    }
}
