using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentRepo
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            try
            {
                var maxId = await _context.Students.MaxAsync(s => s.Id);
                student.Id = (int.Parse(maxId) + 1).ToString();
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student == null)
                {
                    return false;
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync(int page, int pageSize, string? name, string? id)
        {
            try
            {
                var students = await _context.Students
                    .Where(s => (name == null || s.Name.Contains(name)) && (id == null || s.Id == id))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return students;
            }
            catch (Exception)
            {
                return new List<Student>();
            }
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);

                return student;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Student?>> GetStudentsByNameAsync(string name)
        {
            try
            {
                var students = await _context.Students.Where(s => s.Name.Contains(name)).ToListAsync();
                return students;
            }
            catch (Exception)
            {
                return new List<Student?>();
            }
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
