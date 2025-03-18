using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using StudentManagement.DAL.Data.Utils;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<string, int> lastestId = new Dictionary<string, int>();

       


        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // Methods

        public async Task<Result<IEnumerable<Student>>> AddStudentAsync(IEnumerable<Student> students)
        {
            try
            {
                await _context.Students.AddRangeAsync(students);
                return Result<IEnumerable<Student>>.Ok();
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

        public async Task<Result<(IEnumerable<Student> students, int total)>> GetAllStudentsAsync(int page, int pageSize, string? key)
        {
            try
            {
                var students = _context.Students
                    .Include(s => s.Faculty)
                    .Include(s => s.Program)
                    .Include(s => s.Status)
                    .Include(s => s.Identity)
                    .Where(s => key == null || s.Name.Contains(key) || s.Id.Contains(key));

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
        

        /// <summary>
        /// Generate student ID
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        private async Task<string> GenerateStudentId(int course)
        {
            var idCourseYear = String.Format("{0:D2}", course % 100);
            var idLatest = 0;
            if (lastestId.ContainsKey(idCourseYear)) idLatest = lastestId[idCourseYear];
            else
            {
                var latestStudent = await _context.Students
                    .Where(s => s.Id.Contains(idCourseYear))
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync();
                if (latestStudent is null) idLatest = 0;
                else idLatest = int.Parse(latestStudent.Id[^4..]);
            }

            int nextIndex = idLatest + 1;
            return $"{idCourseYear}{nextIndex:D4}";
        }


        
    }
}