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

        public async Task<StudentStatus?> AddStudentStatusAsync(StudentStatus studentStatus)
        {
            await _context.StudentStatuses.AddAsync(studentStatus);
            await _context.SaveChangesAsync();
            return studentStatus;
        }

        public async Task<IEnumerable<StudentStatus>> GetAllStudentStatusesAsync()
        {
            var statuses = await _context.StudentStatuses.ToListAsync();
            return statuses;
        }

        public async Task<StudentStatus?> GetStudentStatusByIdAsync(Guid id)
        {
            var status = await _context.StudentStatuses.FindAsync(id);
            return status;
        }

        public async Task<StudentStatus?> GetStudentStatusByIdAsync(string id) => await GetStudentStatusByIdAsync(id.ToGuid());

        public async Task<StudentStatus?> GetStudentStatusByNameAsync(string name)
        {
            var status = await _context.StudentStatuses.FirstOrDefaultAsync(s => s.Name == name);
            return status;
        }

        public async Task<StudentStatus?> UpdateStudentStatusAsync(StudentStatus studentStatus)
        {
            _context.StudentStatuses.Update(studentStatus);
            await _context.SaveChangesAsync();
            return studentStatus;
        }

        public async Task DeleteStudentStatusAsync(Guid statusId)
        {
            var studentStatus = await _context.StudentStatuses.FindAsync(statusId);
            if (studentStatus is null) return;
            _context.StudentStatuses.Remove(studentStatus);
            await _context.SaveChangesAsync();
        }


        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }
    }
}