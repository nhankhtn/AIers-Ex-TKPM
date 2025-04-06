using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.CourseRepo
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            _context.Courses.Remove(course!);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return courses;
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            return course;
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var updatedCourse = _context.Courses.Find(course.CourseId);
            
            if(updatedCourse == null)
            {
                throw new Exception("Course not found");
            }

            updatedCourse.CourseName = course.CourseName;
            updatedCourse.Credits = course.Credits;
            updatedCourse.Description = course.Description;
            updatedCourse.RequiredCourseId = course.RequiredCourseId;
            updatedCourse.DeletedAt = course.DeletedAt;
            updatedCourse.CreatedAt = course.CreatedAt;
            updatedCourse.FacultyId = course.FacultyId;
            _context.Courses.Update(updatedCourse);
            await _context.SaveChangesAsync();
        }
    }
}
