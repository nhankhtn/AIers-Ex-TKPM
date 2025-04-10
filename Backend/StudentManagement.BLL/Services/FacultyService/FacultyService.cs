using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.DTOs.Faculty;
using StudentManagement.BLL.DTOs.Program;
using StudentManagement.BLL.DTOs.StudentStatus;
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

        public async Task<Result<FacultyDTO?>> AddFacultyAsync(FacultyDTO facultyDTO)
        {
            try
            {
                var faculty = _mapper.Map<Faculty>(facultyDTO);
                var f = await _facultyRepository.AddFacultyAsync(faculty);
                return Result<FacultyDTO?>.Ok(_mapper.Map<FacultyDTO>(f));
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_faculties_name"))
            {
                return Result<FacultyDTO?>.Fail("DUPLICATE_FACULTY_NAME");
            }
            catch (Exception ex)
            {
                return Result<FacultyDTO?>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<FacultyDTO>> UpdateFacultyAsync(string id, FacultyDTO FacultyDTO)
        {
            try
            {
                FacultyDTO.Id = id;
                var studentStatus = _mapper.Map<StudentStatus>(FacultyDTO);
                var existingStudentStatus = await _facultyRepository.GetFacultyByIdAsync(id.ToGuid());
                if (existingStudentStatus == null)
                {
                    return Result<FacultyDTO>.Fail("404", "Student Status not found");
                }

                foreach (var prop in typeof(StudentStatus).GetProperties())
                {
                    var value = prop.GetValue(studentStatus);
                    if (value is null) continue;
                    if (prop.GetValue(existingStudentStatus) == value) continue;
                    prop.SetValue(existingStudentStatus, value);
                }

                var res = await _facultyRepository.UpdateFacultyAsync(existingStudentStatus);
                return Result<FacultyDTO>.Ok(_mapper.Map<FacultyDTO>(res));
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_faculties_name"))
            {
                return Result<FacultyDTO>.Fail("DUPLICATE_FACULTY_NAME");
            }
            catch (Exception ex)
            {
                return Result<FacultyDTO>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<FacultyDTO>>> GetAllFacultiesAsync()
        {
            try
            {
                var faculties = await _facultyRepository.GetAllFacultiesAsync();
                return Result<IEnumerable<FacultyDTO>>.Ok(_mapper.Map<IEnumerable<FacultyDTO>>(faculties));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FacultyDTO>>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<string>> DeleteFacultyAsync(string id)
        {
            try
            {
                await _facultyRepository.DeleteFacultyAsync(id.ToGuid());
                return Result<string>.Ok(id);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("500", ex.Message);
            }
        }


    }
}
