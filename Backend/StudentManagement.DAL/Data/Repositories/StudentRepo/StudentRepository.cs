﻿using Microsoft.Data.SqlClient;
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
            var student = await _context.Students.FindAsync(studentId);
            if (student is null) return;
            student.IsDeleted = true;

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Student> students, int total)> GetAllStudentsAsync(
            int? page = null,
            int? pageSize = null,
            string? faculty = null,
            string? program = null,
            string? status = null,
            string? key = null)
        {
            var query = _ = _context.Students
                .Include(s => s.Faculty)
                .Include(s => s.Program)
                .Include(s => s.Status)
                .Include(s => s.Identity)
                .AsQueryable();

            if (!string.IsNullOrEmpty(faculty))
            {
                query = query.Where(s => s.Faculty.Name.Contains(faculty));
            }

            if (!string.IsNullOrEmpty(program))
            {
                query = query.Where(s => s.Program.Name.Contains(program));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(s => s.Status.Name.Contains(status));
            }

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(s => s.Name.Contains(key));
            }

            query = query.Where(s => !s.IsDeleted);

            var total = await query.CountAsync();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var students = await query.ToListAsync();
            return (students, total);
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            
            var student = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .FirstOrDefaultAsync(s => s.Id == studentId && !s.IsDeleted);
            return student;
        }


        public async Task<IEnumerable<Student?>> GetStudentsByNameAsync(string name)
        {
            var students = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .Where(s => s.Name.Contains(name) && !s.IsDeleted).ToListAsync();
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
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email && !s.IsDeleted);
            return student;
        }

        public async Task<Student?> GetStudentByPhoneAsync(string phone)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Phone == phone && !s.IsDeleted);
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

        public async Task<string> GetStudentNameAsync(string id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            return student?.Name ?? string.Empty;
        }

    }
} 