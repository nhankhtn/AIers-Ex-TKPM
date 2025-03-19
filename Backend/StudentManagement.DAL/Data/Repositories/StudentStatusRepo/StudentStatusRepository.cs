using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            catch (Exception)
            {
                return Result<StudentStatus>.Fail("ADD_STUDENT_STATUS_FAILED", "Add student failed");
            }
        }

        public async Task<Result<IEnumerable<StudentStatus>>> GetAllStudentStatusesAsync()
        {
            try
            {
                var statuses = await _context.StudentStatuses.ToListAsync();
                return Result<IEnumerable<StudentStatus>>.Ok(statuses);
            }
            catch (Exception)
            {
                return Result<IEnumerable<StudentStatus>>.Fail("GET_STUDENT_STATUS_FAILED", "Failed to fetch student statuses");
            }
        }

        public async Task<Result<StudentStatus?>> GetStudentStatusByIdAsync(Guid id)
        {
            try
            {
                var status = await _context.StudentStatuses.FindAsync(id);
                return Result<StudentStatus?>.Ok(status);
            }
            catch (Exception)
            {
                return Result<StudentStatus?>.Fail("GET_STUDENT_STATUS_FAILED", "Failed to fetch student status by ID");
            }
        }

        public async Task<Result<StudentStatus?>> GetStudentStatusByIdAsync(string id) => await GetStudentStatusByIdAsync(Guid.Parse(id));

        public async Task<Result<StudentStatus?>> GetStudentStatusByNameAsync(string name)
        {
            try
            {
                var status = await _context.StudentStatuses.FirstOrDefaultAsync(s => s.Name == name);
                return Result<StudentStatus?>.Ok(status);
            }
            catch (Exception)
            {
                return Result<StudentStatus?>.Fail("GET_STUDENT_STATUS_FAILED", "Failed to fetch student status by name");
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
            catch (Exception)
            {
                return Result<StudentStatus>.Fail("UPDATE_STUDENT_STATUS_FAIL", "Update student status failed");
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
            catch (Exception)
            {
                return Result<StudentStatus>.Fail("DELETE_FACULTY_FAILED");
            }
        }
    }
}