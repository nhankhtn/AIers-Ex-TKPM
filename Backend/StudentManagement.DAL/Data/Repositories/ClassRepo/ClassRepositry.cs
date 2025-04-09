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

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            var classes = await _context.Classes.ToListAsync();
            return classes;
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            return classEntity;
        }

        public async Task UpdateClassAsync(Class classEntity)
        {
            var existingClass = _context.Classes.Find(classEntity.Id);
            if (existingClass == null)
            {
                throw new Exception("Class not found");
            }
            
            existingClass.AcademicYear = classEntity.AcademicYear;
            existingClass.CourseId = classEntity.CourseId;
            existingClass.Semester = classEntity.Semester;
            existingClass.TeacherName = classEntity.TeacherName;
            existingClass.MaxStudents = classEntity.MaxStudents;
            existingClass.Room = classEntity.Room;
            existingClass.DayOfWeek = classEntity.DayOfWeek;
            existingClass.StartTime = classEntity.StartTime;
            existingClass.EndTime = classEntity.EndTime;
            existingClass.Deadline = classEntity.Deadline;

            await _context.SaveChangesAsync();
        }
    }
}
