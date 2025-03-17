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

        public async Task<Result<Faculty>> AddFacultyAsync(Faculty faculty)
        {
            try
            {
                faculty.Id = Guid.NewGuid();
                faculty.CreatedAt = DateTime.Now;
                faculty.UpdatedAt = DateTime.Now;
                await _context.Faculties.AddAsync(faculty);
                await _context.SaveChangesAsync();
                var addedFaculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Code == faculty.Code);
                return Result<Faculty>.Ok(addedFaculty);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Faculties_faculty_code") == null)
            {
                return Result<Faculty>.Fail("FACULTY_CODE_EXIST", "Faculty code has already exist");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Faculties_faculty_name") == null)
            {
                return Result<Faculty>.Fail("FACULTY_NAME_EXIST", "Faculty name has already exist");
            }
            catch (Exception)
            {
                return Result<Faculty>.Fail("ADD_FACULTY_FAILED", "Add faculty failed");
            }
        }

        public async Task<Result<IEnumerable<Faculty>>> GetAllFacultiesAsync()
        {
            try
            {
                var faculties = await _context.Faculties.ToListAsync();
                return Result<IEnumerable<Faculty>>.Ok(faculties);
            }
            catch (Exception)
            {
                return Result<IEnumerable<Faculty>>.Fail();
            }

        }

        public async Task<Result<Faculty?>> GetFacultyByIdAsync(int id)
        {
            try
            {
                var faculty = await _context.Faculties.FindAsync(id);
                return Result<Faculty?>.Ok(faculty);
            }
            catch (Exception)
            {
                return Result<Faculty?>.Fail();
            }
        }

        public async Task<Result<Faculty?>> GetFacultyByNameAsync(string name)
        {
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Name == name);
                return Result<Faculty?>.Ok(faculty);
            }
            catch (Exception)
            {
                return Result<Faculty?>.Fail();
            }
        }

        public async Task<Result<Faculty>> UpdateFacultyAsync(Faculty faculty)
        {
            try
            {
                var existingFaculty = await _context.Faculties.FindAsync(faculty.Id);
                if (existingFaculty == null) return Result<Faculty>.Fail(errorCode: "FACULTY_NOT_EXIST", "Faculty does not exist");

                foreach (var prop in typeof(Faculty).GetProperties())
                {
                    var value = prop.GetValue(faculty);
                    if (value is null) continue;
                    if (prop.GetValue(existingFaculty) == value) continue;
                    prop.SetValue(existingFaculty, value);
                }
                existingFaculty.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return Result<Faculty>.Ok(existingFaculty);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Faculties_faculty_code") == null)
            {
                return Result<Faculty>.Fail("FACULTY_CODE_EXIST", "Faculty code has already exist");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Faculties_faculty_name") == null)
            {
                return Result<Faculty>.Fail("FACULTY_NAME_EXIST", "Faculty name has already exist");
            }
            catch (Exception)
            {
                return Result<Faculty>.Fail("UPDATE_FACULTY_FAILED", "Update faculty failed");
            }
        }
    }
}
