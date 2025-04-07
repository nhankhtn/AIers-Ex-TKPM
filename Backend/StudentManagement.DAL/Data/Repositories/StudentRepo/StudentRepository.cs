using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using StudentManagement.Domain.Attributes;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentRepo
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary> 
        /// Feature
        /// </summary>


        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Methods

        public async Task<IEnumerable<Student>> AddStudentAsync(IEnumerable<Student> students)
        {
            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();
            return students;
        }


        public async Task DeleteStudentAsync(string studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student is null) return;
                _context.Students.Remove(student);

                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {

            }
        }

        public async Task<(IEnumerable<Student> students, int total)> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? program, string? status, string? key)
        {
            var students = _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(faculty))
            {
                students = students.Where(s => s.Faculty.Name == faculty);
            }
            if (!string.IsNullOrEmpty(program))
            {
                students = students.Where(s => s.Program.Name == program);
            }
            if (!string.IsNullOrEmpty(status))
            {
                students = students.Where(s => s.Status.Name == status);
            }

            if (!string.IsNullOrEmpty(key))
            {
                var keywords = key.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var keyword in keywords)
                {
                    students = students.Where(s => s.Name.Contains(keyword) || s.Id.Contains(keyword));
                }
            }

            var total = await students.CountAsync();

            var studentPage = await students.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (studentPage, total);
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            
            var student = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .FirstOrDefaultAsync(s => s.Id == studentId);
            return student;
        }


        public async Task<IEnumerable<Student?>> GetStudentsByNameAsync(string name)
        {
            var students = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .Where(s => s.Name.Contains(name)).ToListAsync();
            return students;
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }
        
        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
            return student;
        }

        public async Task<Student?> GetStudentByPhoneAsync(string phone)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Phone == phone);
            return student;
        }

        public async Task<Student?> GetStudentByDocumentNumberAsync(string documentNumber)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Identity.DocumentNumber == documentNumber);
            return student;
        }

        public async Task<int> GetLatestStudentIdAsync(int course)
        {
            // Lấy 2 số cuối của năm từ course
            string idPrefix = course.ToString()[2..];

            // Tìm sinh viên có Id bắt đầu bằng prefix và lấy Id lớn nhất
            var student = await _context.Students
                .Where(s => s.Id.StartsWith(idPrefix))
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            return student is null ? 0 : int.Parse(student.Id[4..]);
        }

    }
} 