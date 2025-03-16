using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentStatusRepo
{
    public class StudentStatusRepository : IStudentStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddStudentStatusAsync(StudentStatus studentStatus)
        {
            try
            {
                await _context.StudentStatuses.AddAsync(studentStatus);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<StudentStatus>> GetAllStudentStatusesAsync()
        {
            try
            {
                return await _context.StudentStatuses.ToListAsync();
            }
            catch (Exception)
            {
                return new List<StudentStatus>();
            }
        }

        public async Task<StudentStatus?> GetStudentStatusByIdAsync(int id)
        {
            try
            {
                return await _context.StudentStatuses.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StudentStatus?> GetStudentStatusByNameAsync(string name)
        {
            try
            {
                return await _context.StudentStatuses.FirstOrDefaultAsync(s => s.Name == name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateStudentStatusAsync(StudentStatus studentStatus)
        {
            try
            {
                var existingStatus = await _context.StudentStatuses.FindAsync(studentStatus.Id);
                if (existingStatus == null) return false;

                _context.Entry(existingStatus).CurrentValues.SetValues(studentStatus);
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