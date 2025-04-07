using AutoMapper;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.DAL.Data.Repositories.CourseRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.CourseService
{
    public class CourseService: ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<AddCourseDTO>> AddCourseAsync(AddCourseDTO courseDTO)
        {
            try
            {
                var course = _mapper.Map<Course>(courseDTO);

                if (courseDTO.RequiredCourseId != null)
                {
                    var existingCourse = await _courseRepository.GetCourseByIdAsync((int)course.RequiredCourseId!);
                    if (existingCourse == null)
                    {
                        return Result<AddCourseDTO>.Fail("404", "Required course not found");
                    }
                }

                var c = await _courseRepository.AddCourseAsync(course);
                return Result<AddCourseDTO>.Ok(_mapper.Map<AddCourseDTO>(c));
            }
            catch(Exception ex)
            {
                return Result<AddCourseDTO>.Fail("500", ex.Message);
            }
        }

        public async Task<Result<int>> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return Result<int>.Fail("404", "Course not found");
            }
            // accept deleted within 30 minutes start created at
            if (course.CreatedAt.AddMinutes(30) < DateTime.Now)
            {
                return Result<int>.Fail("400", "Course cannot be deleted after 30 minutes");
            }
            
            try
            {
                var hasAnyClass = await _courseRepository.HasAnyClassesAsync(courseId); // check if course has any classes opens
                // if course has any classes opens, add deleted_at
                if (hasAnyClass)
                {
                    course.DeletedAt = DateTime.Now;
                    await _courseRepository.UpdateCourseAsync(course);
                }
                else
                {
                    await _courseRepository.DeleteCourseAsync(courseId);
                }
                var res = Result<int>.Ok(courseId);
                return res;
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("500", ex.Message);
            }

        }
    }
    
}
