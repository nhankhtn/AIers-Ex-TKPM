﻿using Microsoft.EntityFrameworkCore;
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
            var classes = await _context.Classes.Where(cl => cl.CourseId == courseId).Include(cl => cl.Students).ToListAsync();
            foreach(var cl in classes)
            {
                if (cl.Students != null && cl.Students.Count > 0)
                    return false;
            }
            return true;
        }

        public async Task DeleteCourseAsync(string courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                throw new Exception("Không tìm thấy khóa học.");
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
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
