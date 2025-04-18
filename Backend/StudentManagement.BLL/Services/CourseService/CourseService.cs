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
                    var existingCourse = await _courseRepository.GetCourseByIdAsync(course.RequiredCourseId!);
                    if (existingCourse == null)
                    {
                        return Result<AddCourseDTO>.Fail("PRE_COURSE_NOT_FOUND", "Khóa học tiên quyết không tồn tại.");
                    }
                    
                }


                var c = await _courseRepository.AddCourseAsync(course);
                return Result<AddCourseDTO>.Ok(_mapper.Map<AddCourseDTO>(c));
            }
            catch(Exception ex)
            {
                return Result<AddCourseDTO>.Fail("ADD_COURSE_FAILED", ex.Message);
            }
        }

        public async Task<Result<string>> DeleteCourseAsync(string courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return Result<string>.Fail("COURSE_NOT_FOUND", ErrorMessages.CourseNotFound);
            }
            // accept deleted within 30 minutes start created at
            if (course.CreatedAt.AddMinutes(30) < DateTime.Now)
            {
                return Result<string>.Fail("DELETE_COURSE_FAILED", "Đã quá 30 phút kể từ khi lập khóa học.");
            }
            
            try
            {
                var hasAnyClass = await _courseRepository.HasAnyClassesAsync(courseId); // check if course has any classes opens
               
                // if course has any classes opens, add deleted_at
                if (hasAnyClass)
                {
                    course.DeletedAt = DateTime.Now;
                    await _courseRepository.UpdateCourseAsync(course);
                    return Result<string>.Fail("DELETE_COURSE_FAILED", "Course already has class. Status will be changed to Deactivated");
                }
                else
                {
                    await _courseRepository.DeleteCourseAsync(courseId);
                }

                var res = Result<string>.Ok(courseId);
                return res;
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("DELETE_COURSE_FAILED", ex.Message);
            }

        }

        public async Task<Result<GetAllCoursesDTO>> GetAllCourseAsync(int page, int limit, Guid? facultyId, string? courseId, bool isDeleted)
        {
            try
            {
                var (courses, total) = await _courseRepository.GetAllCoursesAsync(page, limit, facultyId, courseId, isDeleted);
                var courseDTOs = new List<GetCourseDTO>();
                foreach (var course in courses)
                {
                    courseDTOs.Add(_mapper.Map<GetCourseDTO>(course));
                }
                var res = new GetAllCoursesDTO
                { 
                    Data = courseDTOs,
                    Total = total
                };

                return Result<GetAllCoursesDTO>.Ok(res);
            }
            catch (Exception ex)
            {
                return Result<GetAllCoursesDTO>.Fail("GET_COURSES_FAILED", ex.Message);
            }
        }

        public async Task<Result<GetCourseDTO>> GetAllCourseByIdAsync(string courseId)
        {
            try
            {
                var course = await _courseRepository.GetCourseByIdAsync(courseId);
                if (course is null)
                    return Result<GetCourseDTO>.Fail("COURSE_NOT_FOUND", ErrorMessages.CourseNotFound);
                return Result<GetCourseDTO>.Ok(_mapper.Map<GetCourseDTO>(course));
            }
            catch(Exception ex)
            {
                return Result<GetCourseDTO>.Fail("GET_COURSE_FAILED", ex.Message);
            }
        
        }

        public async Task<Result<UpdateCourseDTO>> UpdateCourseByIdAsync(string courseId, UpdateCourseDTO courseDTO)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if(course is null)
            {
                return Result<UpdateCourseDTO>.Fail("COURSE_NOT_FOUND", ErrorMessages.CourseNotFound);
            }
            var checkHasStudentInCouse = await _courseRepository.CheckHasAnyStudentInCourseAsync(courseId);
            if (!checkHasStudentInCouse && courseDTO.Credits != course.Credits)
            {
                return Result<UpdateCourseDTO>.Fail("UPDATE_COURSE_FAILED", "The course already has enrolled students");
            }
            try
            {
               
                course.Credits = courseDTO.Credits;
                course.CourseName = courseDTO.CourseName;
                course.Description = courseDTO.Description;
                course.FacultyId = courseDTO.FacultyId;
                var result = await _courseRepository.UpdateCourseAsync(course);

                return Result<UpdateCourseDTO>.Ok(courseDTO);
                
            }
            catch(Exception ex)
            {
                return Result<UpdateCourseDTO>.Fail("UPDATE_COURSE_FAILED", ex.Message);
            }


        }
    }
    
}
