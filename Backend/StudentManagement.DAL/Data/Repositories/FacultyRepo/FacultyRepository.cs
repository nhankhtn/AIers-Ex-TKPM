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

        public async Task<Result<Faculty>> AddFacultyAsync(Faculty faculty)
        {
            try
            {
                faculty.Id = Guid.NewGuid();
                await _context.Faculties.AddAsync(faculty);
                await _context.SaveChangesAsync();
                var addedFaculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == faculty.Id);
                return Result<Faculty>.Ok(addedFaculty);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_faculty_name") == null)
            {
                return Result<Faculty>.Fail("FACULTY_NAME_EXIST", "Faculty name has already exist");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Faculty>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Faculty>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Faculty>.Fail("ADD_FACULTY_FAILED", ex.Message);
            }
        }

        public async Task<Result<Faculty>> DeleteFacultyAsync(Faculty faculty)
        {
            try
            {
                var existingFaculty = await _context.Faculties.Where(f => f.Id == faculty.Id || f.Name == faculty.Name).FirstOrDefaultAsync();
                if (existingFaculty is null) return Result<Faculty>.Fail("FACULTY_NOT_EXIST", "Faculty does not exist");
                _context.Faculties.Remove(existingFaculty);
                await _context.SaveChangesAsync();
                return Result<Faculty>.Ok(existingFaculty);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Faculty>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Faculty>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Faculty>.Fail("DELETE_FACULTY_FAILED", ex.Message); 
            }
        }

        public async Task<Result<IEnumerable<Faculty>>> GetAllFacultiesAsync()
        {
            try
            {
                var faculties = await _context.Faculties.ToListAsync();
                return Result<IEnumerable<Faculty>>.Ok(faculties);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<IEnumerable<Faculty>>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<IEnumerable<Faculty>>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Faculty>>.Fail("GET_FACULTY_FAILED", ex.Message);
            }

        }

        public async Task<Result<Faculty?>> GetFacultyByIdAsync(Guid id)
        {
            try
            {
                var faculty = await _context.Faculties.FindAsync(id);
                return Result<Faculty?>.Ok(faculty);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Faculty?>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Faculty?>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Faculty?>.Fail("GET_FACULTY_FAILED", ex.Message);
            }
        }

        public async Task<Result<Faculty?>> GetFacultyByIdAsync(string id) => await GetFacultyByIdAsync(id.ToGuid());

        public async Task<Result<Faculty?>> GetFacultyByNameAsync(string name)
        {
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Name == name);
                return Result<Faculty?>.Ok(faculty);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Faculty?>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Faculty?>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Faculty?>.Fail("GET_FACULTY_FAILED", ex.Message);
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
                //existingFaculty.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return Result<Faculty>.Ok(existingFaculty);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_faculty_name") == null)
            {
                return Result<Faculty>.Fail("FACULTY_NAME_EXIST", "Faculty name has already exist");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Faculty>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Faculty>.Fail("DB_ERROR", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Faculty>.Fail("UPDATE_FACULTY_FAILED", ex.Message);
            }
        }


        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }

    }
}
