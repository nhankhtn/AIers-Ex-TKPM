using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
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
        public async Task<Course> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> CheckHasAnyStudentInCourseAsync(string courseId)
        {
            var query = await (from c in _context.Classes
                        join s in _context.ClassStudents on c.Id equals s.ClassId
                        where c.CourseId == courseId
                        select s.StudentId).AnyAsync();
            return query;
        }

        public async Task DeleteCourseAsync(string courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }
            _context.Courses.Remove(course!);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Course>, int)> GetAllCoursesAsync(int page, int limit, Guid? facultyId, string? courseId, bool isDeleted)
        {
            //var courses = await _context.Courses.ToListAsync();
            //var query = await (from c in _context.Courses
            //            join f in _context.Faculties on c.FacultyId equals f.Id
            //            join rc in _context.Courses on c.RequiredCourseId equals rc.CourseId
            //            select new { c, f, rc }).ToListAsync();
            //var courses = new List<Course>();
            //for(int i = 0; i < query.Count; i++)
            //{
            //    query[i].c.RequiredCourse = new();
            //    query[i].c.Faculty = new();
            //    query[i].c.RequiredCourse!.CourseName = query[i].rc.CourseName;
            //    query[i].c.Faculty.Name = query[i].f.Name;
            //    courses.Add(query[i].c);
            //}
            var courses = _context.Courses.Include(c => c.RequiredCourse).Include(c => c.Faculty).AsQueryable();
            // chuyen doi thanh AsQueryAble de co the dung LINQ nang cao nhu where, select, order by
            if (facultyId != null)
            {
                courses = courses.Where(c => c.FacultyId == facultyId);
            }
            if (!string.IsNullOrWhiteSpace(courseId))
            {
                courses = courses.Where(c => c.CourseId.Contains(courseId!));
            }
            if (isDeleted)
            {
                courses = courses.Where(c => c.DeletedAt != null);
            }
            var total = courses.Count();
            var res = await courses.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return (res, total);
        }

        public async Task<Course?> GetCourseByIdAsync(string courseId)
        {
            var course = await _context.Courses.Include(c => c.RequiredCourse).Include(c => c.Faculty).FirstOrDefaultAsync(c => c.CourseId == courseId);
            return course;
        }

        public async Task<bool> HasAnyClassesAsync(string courseId)
        {
           var result = await _context.Classes.AnyAsync(c => c.CourseId == courseId);
            return result;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
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

            return course;
        }
    }
}
