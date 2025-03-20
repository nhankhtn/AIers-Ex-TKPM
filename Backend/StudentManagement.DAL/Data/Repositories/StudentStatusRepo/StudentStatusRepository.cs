using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentManagement.DAL.Data.Repositories.StudentStatusRepo
{
    public class StudentStatusRepository : IStudentStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<StudentStatus>> AddStudentStatusAsync(StudentStatus studentStatus)
        {
            try
            {
                studentStatus.Id = Guid.NewGuid();
                await _context.StudentStatuses.AddAsync(studentStatus);
                await _context.SaveChangesAsync();
                var addedStatus = await _context.StudentStatuses.FirstOrDefaultAsync(s => s.Id == studentStatus.Id);
                return Result<StudentStatus>.Ok(addedStatus);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_student_status_name") == true)
            {
                return Result<StudentStatus>.Fail("STUDENT_STATUS_NAME_EXIST", "Student status name already exists");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex)
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<StudentStatus>.Fail("ADD_STUDENT_STATUS_FAILED", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<StudentStatus>>> GetAllStudentStatusesAsync()
        {
            try
            {
                var statuses = await _context.StudentStatuses.ToListAsync();
                return Result<IEnumerable<StudentStatus>>.Ok(statuses);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<IEnumerable<StudentStatus>>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex)
            {
                return Result<IEnumerable<StudentStatus>>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<StudentStatus>>.Fail("GET_STUDENT_STATUS_FAILED", ex.Message);
            }
        }

        public async Task<Result<StudentStatus?>> GetStudentStatusByIdAsync(Guid id)
        {
            try
            {
                var status = await _context.StudentStatuses.FindAsync(id);
                return Result<StudentStatus?>.Ok(status);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<StudentStatus?>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex)
            {
                return Result<StudentStatus?>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<StudentStatus?>.Fail("GET_STUDENT_STATUS_FAILED", ex.Message);
            }
        }

        public async Task<Result<StudentStatus?>> GetStudentStatusByIdAsync(string id) => await GetStudentStatusByIdAsync(id.ToGuid());

        public async Task<Result<StudentStatus?>> GetStudentStatusByNameAsync(string name)
        {
            try
            {
                var status = await _context.StudentStatuses.FirstOrDefaultAsync(s => s.Name == name);
                return Result<StudentStatus?>.Ok(status);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<StudentStatus?>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex)
            {
                return Result<StudentStatus?>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<StudentStatus?>.Fail("GET_STUDENT_STATUS_FAILED", ex.Message);
            }
        }

        public async Task<Result<StudentStatus>> UpdateStudentStatusAsync(StudentStatus studentStatus)
        {
            try
            {
                var existingStatus = await _context.StudentStatuses.FindAsync(studentStatus.Id);
                if (existingStatus == null)
                    return Result<StudentStatus>.Fail("STUDENT_STATUS_NOT_FOUND", "Student status not found");

                foreach(var prop in typeof(StudentStatus).GetProperties())
                {
                    var value = prop.GetValue(studentStatus);
                    if (value is null) continue;
                    if (prop.GetValue(existingStatus) == value) continue;
                    prop.SetValue(existingStatus, value);
                }

                await _context.SaveChangesAsync();
                return Result<StudentStatus>.Ok(existingStatus);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_student_status_name") == true)
            {
                return Result<StudentStatus>.Fail("STUDENT_STATUS_NAME_EXIST", "Student status name already exists");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (SqlException ex)
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<StudentStatus>.Fail("UPDATE_STUDENT_STATUS_FAIL", ex.Message);
            }
        }

        public async Task<Result<StudentStatus>> DeleteStudentStatusAsync(StudentStatus studentStatus)
        {
            try
            {
                var existingStudentStatus = await _context.StudentStatuses.Where(s => s.Id == studentStatus.Id || s.Name == studentStatus.Name).FirstOrDefaultAsync();
                if (existingStudentStatus is null) return Result<StudentStatus>.Fail("FACULTY_NOT_EXIST", "Faculty does not exist");
                _context.StudentStatuses.Remove(existingStudentStatus);
                await _context.SaveChangesAsync();
                return Result<StudentStatus>.Ok(existingStudentStatus);
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<StudentStatus>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<StudentStatus>.Fail("DELETE_FACULTY_FAILED", ex.Message);
            }
        }


        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }
    }
}