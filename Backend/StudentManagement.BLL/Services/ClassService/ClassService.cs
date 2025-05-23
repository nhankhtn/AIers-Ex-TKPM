﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.DAL.Data.Repositories.ClassRepo;
using StudentManagement.DAL.Data.Repositories.ClassStudentRepo;
using StudentManagement.DAL.Data.Repositories.CourseRepo;
using StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassService
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IClassStudentRepository _classStudentRepostory;
        private readonly IRegisterCancellationHistoryRepository _registerCancellationHistoryRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, 
            ICourseRepository courseRepository, 
            IClassStudentRepository classStudentRepository, 
            IRegisterCancellationHistoryRepository registerCancellationHistoryRepository,
            IMapper mapper)
        {
            _classRepository = classRepository;
            _courseRepository = courseRepository;
            _classStudentRepostory = classStudentRepository;
            _registerCancellationHistoryRepository = registerCancellationHistoryRepository;
            _mapper = mapper;
        }


        public async Task<Result<GetClassDTO>> AddClassAsync(AddClassDTO addClassDTO)
        {
            try
            {
                var course = await _courseRepository.GetCourseByIdAsync(addClassDTO.CourseId);
                if (course is null || course.DeletedAt < DateTime.Now) 
                    return Result<GetClassDTO>.Fail("COURSE_NOT_FOUND");
                var _class = await _classRepository.AddClassAsync(_mapper.Map<Class>(addClassDTO));
                return Result<GetClassDTO>.Ok(_mapper.Map<GetClassDTO>(_class));
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("duplicate"))
            {
                return Result<GetClassDTO>.Fail("DUPLICATE_CLASS_ID");
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("FK_classes_courses_course_id"))
            {
                return Result<GetClassDTO>.Fail("COURSE_NOT_FOUND");
            }
            catch (DbUpdateException ex) when (ex.InnerException is not null && ex.InnerException.Message.Contains("truncated"))
            {
                return Result<GetClassDTO>.Fail("COURSE_ID_TOO_LONG", "Mã lớp học dưới 10 ký tự.");
            }
            catch (Exception ex)
            {
                return Result<GetClassDTO>.Fail("ADD_CLASS_FAILED", ex.Message);
            }
        }
        //FK_classes_courses_course_id

        public async Task<Result<GetClassDTO>> GetClassAsync(string classId)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<GetClassDTO>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);

                var classDTO = _mapper.Map<GetClassDTO>(_class);
                classDTO.CurrentStudents = await _classStudentRepostory.GetNumberOfStudentsInClassAsync(classId);

                return Result<GetClassDTO>.Ok(classDTO);
            }
            catch (Exception ex)
            {
                return Result<GetClassDTO>.Fail("GET_CLASS_FAILED", ex.Message);
            }
        }

        public async Task<Result<GetClassesDTO>> GetClassesAsync(string? classId = null, int? semester = null, int? year = null, int? page = null, int? limit = null)
        {
            try
            {
                var classes = await _classRepository.GetClassesAsync(classId, semester, year, page, limit);

                var data = _mapper.Map<IEnumerable<GetClassDTO>>(classes);
                foreach(var c in data)
                {
                    c.CurrentStudents = await _classStudentRepostory.GetNumberOfStudentsInClassAsync(c.ClassId ?? "");
                }

                return Result<GetClassesDTO>.Ok(new GetClassesDTO {
                     Data = data,
                     Total = classes.Count()
                });
            }
            catch (Exception ex)
            {
                return Result<GetClassesDTO>.Fail("GET_CLASSES_FAILED", ex.Message);
            }
        }

        public async Task<Result<GetClassDTO>> DeleteClassAsync(string id)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(id);
                if (_class is null)
                    return Result<GetClassDTO>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);
                await _classRepository.DeleteClassAsync(id);
                return Result<GetClassDTO>.Ok(new GetClassDTO { ClassId = id });
            }
            catch (Exception ex)
            {
                return Result<GetClassDTO>.Fail("DELETE_CLASS_FAILED", ex.Message);
            }
        }

        public async Task<Result<GetClassDTO>> UpdateClassAsync(string classId, UpdateClassDTO updateClassDTO)
        {
            try
            {
                var _class = await _classRepository.GetClassByIdAsync(classId);
                if (_class is null)
                    return Result<GetClassDTO>.Fail("CLASS_NOT_FOUND", ErrorMessages.ClassNotFound);
                _mapper.Map(updateClassDTO, _class);
                await _classRepository.UpdateClassAsync(_class);
                return Result<GetClassDTO>.Ok(_mapper.Map<GetClassDTO>(_class));
            }
            catch (Exception ex)
            {
                return Result<GetClassDTO>.Fail("UPDATE_CLASS_FAILED", ex.Message);
            }
        }
    }
}
