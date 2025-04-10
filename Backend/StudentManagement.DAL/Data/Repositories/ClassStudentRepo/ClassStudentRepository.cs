using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task AddClassStudentAsync(ClassStudent classStudent)
        {
            _context.ClassStudents.Add(classStudent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassStudentAsync(int classId, string studentId)
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

        public async Task<IEnumerable<ClassStudent>> GetClassStudentsAsync(int? classId = null, string? studentId = null, int? page = null, int? limit = null)
        {
            var query = _context.ClassStudents.AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(cs => cs.ClassId == classId.Value);
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

        public async Task<ClassStudent?> GetClassStudentByIdAsync(int classId, string studentId)
        {
            //var classStudent = await _context.ClassStudents.FindAsync(id);
            var classStudent = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);
            return classStudent;
        }

        public async Task UpdateClassStudentAsync(ClassStudent classStudent)
        {
            //var existingClassStudent = _context.ClassStudents.Find(classStudent.ClassId);
            var existingClassStudent = await _context.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classStudent.ClassId && cs.StudentId == classStudent.StudentId);
            if (existingClassStudent == null)
            {
                throw new ArgumentNullException("ClassStudent not found");
            }
            existingClassStudent.Score = classStudent.Score;
            _context.ClassStudents.Update(existingClassStudent);
            await _context.SaveChangesAsync();
        }
    }
}
