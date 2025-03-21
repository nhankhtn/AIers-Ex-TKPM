using Microsoft.Data.SqlClient;
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
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<IEnumerable<Student>>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<IEnumerable<Student>>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception? ex)
            {
                return Result<IEnumerable<Student>>.Fail("ADD_FAILED", ex.Message);
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
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("DELETE_FAILED", ex.Message);
            }
        }

        public async Task<Result<(IEnumerable<Student> students, int total)>> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? program, string? status, string? key)
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

                return Result<(IEnumerable<Student> students, int total)>.Ok((studentPage, total));
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<(IEnumerable<Student> students, int total)>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<(IEnumerable<Student> students, int total)>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<(IEnumerable<Student> students, int total)>.Fail("GET_STUDENTS_FAILED", ex.Message);
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
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<Student?>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<Student?>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<Student?>.Fail(errorCode: "NOT_FOUND_STUDENT", errorMessage: ex.Message);
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
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<IEnumerable<Student?>>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<IEnumerable<Student?>>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Student?>>.Fail("NOT_FOUND_STUDENT", ex.Message);
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
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("UPDATE_FAILED", ex.Message);
            }
        }
        

       


        public async Task<Result<string>> IsEmailDuplicateAsync(string email)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
                return student is null ? Result<string>.Ok() : Result<string>.Fail("EMAIL_EXIST", "Email has already existed");
            }
            catch (SqlException ex) when (IsConnectDatabaseFailed(ex))
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Database connection error. Please try again later.");
            }
            catch (SqlException ex)
            {
                return Result<string>.Fail("DB_CONNECT_FAILED", "Something went wrong. Please try again later.");
            }
            catch (Exception)
            {
                return Result<string>.Fail("CHECK_FAILED", "Check unique constrains failed");
            }
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


        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }
    }
} 