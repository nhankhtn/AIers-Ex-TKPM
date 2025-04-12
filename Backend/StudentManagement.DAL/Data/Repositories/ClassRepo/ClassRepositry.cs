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

        public async Task DeleteClassAsync(string id)
        {
            var existingClass = _context.Classes.Find(id);
            if(existingClass == null)
            {
                throw new Exception("Class not found");
            }
            _context.Classes.Remove(existingClass);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesAsync(string? classId, int? semeter = null, int? page = null, int? limit = null)
        {
            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(classId))
            {
                query = query.Where(c => c.ClassId.Contains(classId));
            }

            if (semeter != null)
            {
                query = query.Where(c => c.Semester == semeter);
            }

            if (page.HasValue && limit.HasValue)
            {
                query = query.Skip((page.Value - 1) * limit.Value).Take(limit.Value);
            }

            return await query.Include(c => c.Course).ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(string id)
        {
            var classEntity = await _context.Classes
                .Where(c => c.ClassId == id)
                .Include(c => c.Course)
                .FirstOrDefaultAsync();

            return classEntity;
        }

        public async Task UpdateClassAsync(Class classEntity)
        {
            _context.Update(classEntity);
            await _context.SaveChangesAsync();
        }
    }
}
