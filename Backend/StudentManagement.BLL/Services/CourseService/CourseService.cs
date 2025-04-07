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
                        return Result<AddCourseDTO>.Fail("400", "Required course not found");
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
    }
    
}
