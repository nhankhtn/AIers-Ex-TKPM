using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.FacultyRepo
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _context;

        public FacultyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFacultyAsync(Faculty faculty)
        {
            try
            {

                await _context.Faculties.AddAsync(faculty);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            try
            {
                var faculties = await _context.Faculties.ToListAsync();
                return faculties;
            }
            catch (Exception)
            {
                return new List<Faculty>();
            }

        }

        public async Task<Faculty?> GetFacultyByIdAsync(int id)
        {
            try
            {
                var faculty = await _context.Faculties.FindAsync(id);
                return faculty;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Faculty?> GetFacultyByNameAsync(string name)
        {
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Name == name);
                return faculty;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateFacultyAsync(Faculty faculty)
        {
            try
            {
                var existingFaculty = await _context.Faculties.FindAsync(faculty.Id);
                if (existingFaculty == null) return false;

                _context.Entry(existingFaculty).CurrentValues.SetValues(faculty);
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
