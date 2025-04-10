using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassRepo
{
    public class ClassRepositry : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        public ClassRepositry(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Class?> AddClassAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
            return classEntity;
        }

        public async Task DeleteClassAsync(int id)
        {
            var existingClass = _context.Classes.Find(id);
            if(existingClass == null)
            {
                throw new Exception("Class not found");
            }
            _context.Classes.Remove(existingClass);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesAsync(string? courseId = null, int? page = null, int? limit = null)
        {
            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(courseId))
            {
                query = query.Where(c => c.CourseId == courseId);
            }

            if (page.HasValue && limit.HasValue)
            {
                query = query.Skip((page.Value - 1) * limit.Value).Take(limit.Value);
            }

            return await query.Include(c => c.Course).ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var classEntity = await _context.Classes
                .Where(c => c.Id == id)
                .Include(c => c.Course)
                .FirstOrDefaultAsync();

            return classEntity;
        }

        public async Task UpdateClassAsync(Class classEntity)
        {
            _context.Update(classEntity);
            await _context.SaveChangesAsync();
        }

        //public async Task<IEnumerable<Class?>> GetStudentJoinedClassesAsync(string studentId, string courseId)
        //{
        //    var classes = await _context.Classes
        //        .Include(c => c.ClassStudents)
        //        .ThenInclude(cs => cs.Student)
        //        .Where(c => c.CourseId == courseId && c.ClassStudents.Any(cs => cs.StudentId == studentId))
        //        .ToListAsync();

        //    return classes;
        //}
    }
}
