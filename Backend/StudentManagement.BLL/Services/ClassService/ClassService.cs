using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.DAL.Data.Repositories.ClassRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassService
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }


        public async Task<Result<AddClassDTO>> AddClassAsync(AddClassDTO addClassDTO)
        {
            try
            {
                await _classRepository.AddClassAsync(_mapper.Map<Class>(addClassDTO));
                return Result<AddClassDTO>.Ok(addClassDTO);
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("FK_classes_courses_course_id"))
            {
                return Result<AddClassDTO>.Fail("COURSE_NOT_FOUND");
            }
            catch(Exception)
            {
                return Result<AddClassDTO>.Fail("ADD_CLASS_FAILED");
            }
        }
        //FK_classes_courses_course_id

        public async Task<Result<GetClassDTO>> GetClassAsync(int classId)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<GetClassDTO>.Fail("CLASS_NOT_FOUND");
                return Result<GetClassDTO>.Ok(_mapper.Map<GetClassDTO>(_class));
            }
            catch (Exception)
            {
                return Result<GetClassDTO>.Fail("GET_CLASS_FAILED");
            }
        }

        public async Task<Result<IEnumerable<GetClassDTO>>> GetClassesAsync(string? course = null)
        {
            try
            {
                var classes = await _classRepository.GetClassesAsync(course);
                return Result<IEnumerable<GetClassDTO>>.Ok(_mapper.Map<IEnumerable<GetClassDTO>>(classes));
            }
            catch (Exception)
            {
                return Result<IEnumerable<GetClassDTO>>.Fail("GET_CLASSES_FAILED");
            }
        }

        public async Task<Result<GetClassDTO>> DeleteClassAsync(int id)
        {
            try
            {
                await _classRepository.DeleteClassAsync(id);
                return Result<GetClassDTO>.Ok(new GetClassDTO { Id = id });
            }
            catch (Exception)
            {
                return Result<GetClassDTO>.Fail("DELETE_CLASS_FAILED");
            }
        }

        public async Task<Result<GetClassDTO>> UpdateClassAsync(int classId, UpdateClassDTO updateClassDTO)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<GetClassDTO>.Fail("CLASS_NOT_FOUND");
                _mapper.Map(updateClassDTO, _class);
                await _classRepository.UpdateClassAsync(_class);
                return Result<GetClassDTO>.Ok(_mapper.Map<GetClassDTO>(_class));
            }
            catch (Exception)
            {
                return Result<GetClassDTO>.Fail("UPDATE_CLASS_FAILED");
            }
        }
    }
}
