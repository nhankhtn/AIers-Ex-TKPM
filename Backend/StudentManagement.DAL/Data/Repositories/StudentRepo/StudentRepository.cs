using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using StudentManagement.DAL.Data.Utils;
using StudentManagement.Domain.Attributes;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private Dictionary<int, int> generateIdCache = new Dictionary<int, int>();


        // Methods

        public async Task<Result<IEnumerable<Student>>> AddStudentAsync(IEnumerable<Student> students)
        {
            try
            {
                await _context.Students.AddRangeAsync(students);
                await _context.SaveChangesAsync();
                return Result<IEnumerable<Student>>.Ok(students);
            }
            catch (Exception)
            {
                return Result<IEnumerable<Student>>.Fail("ADD_FAILED", "Add student failed");
            }
        }


        public async Task<Result<string>> DeleteStudentAsync(string studentId)
        {
            try
            {
                var student = await _context.Students.FindAsync(studentId);
                if (student == null)
                {
                    return Result<string>.Fail("STUDENT_NOT_EXIST", "Student does not exist");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return Result<string>.Ok();
            }
            catch (Exception)
            {
                return Result<string>.Fail("DELETE_FAILED", "Delete student failed");
            }
        }

        public async Task<Result<(IEnumerable<Student> students, int total)>> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? key)
        {
            try
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

                if (!string.IsNullOrEmpty(key))
                {
                    students = students.Where(s => s.Name.Contains(key) || s.Id.Contains(key));
                }

                var total = await students.CountAsync();

                var studentPage = await students.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<(IEnumerable<Student> students, int total)>.Ok((studentPage, total));
            }
            catch (Exception)
            {
                return Result<(IEnumerable<Student> students, int total)>.Fail();
            }
        }

        public async Task<Result<Student?>> GetStudentByIdAsync(string studentId)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .FirstOrDefaultAsync(s => s.Id == studentId);
                return Result<Student?>.Ok(student);
            }
            catch (Exception)
            {
                return Result<Student?>.Fail(errorCode: "NOT_FOUND_STUDENT", errorMessage: "Not found student");
            }
        }


        public async Task<Result<IEnumerable<Student?>>> GetStudentsByNameAsync(string name)
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .Where(s => s.Name.Contains(name)).ToListAsync();
                return Result<IEnumerable<Student?>>.Ok(students);
            }
            catch (Exception)
            {
                return Result<IEnumerable<Student?>>.Fail("NOT_FOUND_STUDENT");
            }
        }

        public async Task<Result<string>> UpdateStudentAsync(Student student)
        {
            try
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return Result<string>.Ok();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_students_email") == true)
            {
                return Result<string>.Fail("EMAIL_EXIST", "Email has already existed");
            }
            catch (Exception)
            {
                return Result<string>.Fail("UPDATE_FAILED", "Update student failed");
            }
        }
        

       


        public async Task<Result<string>> IsEmailDuplicateAsync(string email)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
                return student is null ? Result<string>.Ok() : Result<string>.Fail("EMAIL_EXIST", "Email has already existed");
            }
            catch (Exception)
            {
                return Result<string>.Fail("CHECK_FAILED", "Check unique constrains failed");
            }
        }

        public async Task<int> GetLatestStudentIdAsync(int course)
        {
            var student = await _context.Students.Where(s => s.Course == course)
                .OrderByDescending(s => s.Id).FirstOrDefaultAsync();

            return student is null ? 0 : int.Parse(student.Id[4..]);
        }
    }
} 