using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
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

        public async Task<Faculty?> AddFacultyAsync(Faculty faculty)
        {
            faculty.Id = Guid.NewGuid();
            await _context.Faculties.AddAsync(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task DeleteFacultyAsync(Guid faculty)
        {
            var facultyToDelete = await _context.Faculties.FindAsync(faculty);
            if (facultyToDelete is null) return;
            _context.Faculties.Remove(facultyToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            var faculties = await _context.Faculties.ToListAsync();
            return faculties;
        }

        public async Task<Faculty?> GetFacultyByIdAsync(Guid id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            return faculty;
        }

        public async Task<Faculty?> GetFacultyByIdAsync(string id) => await GetFacultyByIdAsync(id.ToGuid());

        public async Task<Faculty?> GetFacultyByNameAsync(string name)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Name == name);
            return faculty;
        }

        public async Task<Faculty?> UpdateFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Update(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        } 
    }
}
