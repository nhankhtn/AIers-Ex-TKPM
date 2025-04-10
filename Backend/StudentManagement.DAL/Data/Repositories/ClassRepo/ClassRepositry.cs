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
        public async Task AddClassAsync(Class classEntity)
        {
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<Class>> GetClassesAsync(string? course = null)
        {
            return await _context.Classes
                    .Where(c => course == null || c.Course.CourseName.Contains(course))
                    .ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            return classEntity;
        }

        public async Task UpdateClassAsync(Class classEntity)
        {
            _context.Update(classEntity);
            await _context.SaveChangesAsync();
        }
    }
}
