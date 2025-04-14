using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassStudentRepo
{
    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public ClassStudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ClassStudent?> AddClassStudentAsync(ClassStudent classStudent)
        {
            _context.ClassStudents.Add(classStudent);
            await _context.SaveChangesAsync();
            return classStudent;
        }

        public async Task DeleteClassStudentAsync(string classId, string studentId)
        {
            //var classStudent = _context.ClassStudents.Find();
            var classStudent = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);
            if (classStudent == null)
            {
                throw new ArgumentNullException("ClassStudent not found");
            }
            _context.ClassStudents.Remove(classStudent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassStudent>> GetClassStudentsAsync(string? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            var query = _context.ClassStudents.AsQueryable();

            if (!string.IsNullOrEmpty(classId))
            {
                query = query.Where(cs => cs.ClassId == classId);
            }

            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(cs => cs.StudentId == studentId);
            }

            if (page.HasValue && limit.HasValue)
            {
                query = query.Skip((page.Value - 1) * limit.Value).Take(limit.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<ClassStudent?> GetClassStudentByIdAsync(string classId, string studentId)
        {
            //var classStudent = await _context.ClassStudents.FindAsync(id);
            var classStudent = await _context.ClassStudents
                .Include(cs => cs.RegisterCancellationHistories)
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);
            return classStudent;
        }

        public async Task UpdateClassStudentAsync(ClassStudent classStudent)
        {
            _context.ClassStudents.Update(classStudent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClassStudent?>> GetClassStudentByIdAndCourseAsync(string studentId, string courseId)
        {
            var classStudents = await _context.ClassStudents
                .Include(c => c.Class)
                .ThenInclude(c => c.Course)
                .Where(cs => cs.StudentId == studentId && cs.Class.CourseId == courseId)
                .ToListAsync();

            return classStudents;
        }

        public Task<int> GetNumberOfStudentsInClassAsync(string classId)
        {
            var numberOfStudents = _context.ClassStudents.Where(cs => cs.ClassId == classId).CountAsync();
            return numberOfStudents;
        }
    }
}
