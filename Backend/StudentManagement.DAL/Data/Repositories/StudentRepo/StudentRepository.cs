using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentRepo
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Student>> AddStudentAsync(Student student)
        {
            try
            {
                var latestStudent = await _context.Students
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync(s => s.Id.StartsWith("22120"));

                int nextId = latestStudent == null ? 1 : int.Parse(latestStudent.Id[^3..]) + 1;

                student.Id = $"22120{nextId:D3}";

                //// check unique email 
                //var studentEmail = await _context.Students.FirstOrDefaultAsync(s => s.Email == student.Email);
                //if (studentEmail != null)
                //{
                //    return Result<string>.Fail("EMAIL_EXISTS", "Email has alreay existed");
                //}


                // 
                var faculty = await _context.Faculties.FindAsync(student.FacultyId);
                if (faculty == null)
                {
                    return Result<Student>.Fail("FACULTY_NOT_EXIST", "Faculty does not exist");
                }
                student.Faculty = faculty;
                var program = await _context.Programs.FindAsync(student.ProgramId);
                if (program == null)
                {
                    return Result<Student>.Fail("PROGRAM_NOT_EXIST", "Program does not exist");
                }
                student.Program = program;
                var status = await _context.StudentStatuses.FindAsync(student.StatusId);
                if (status == null)
                {
                    return Result<Student>.Fail("STATUS_NOT_EXIST", "Status does not exist");
                }
                student.Status = status;

                await _context.Students.AddAsync(student);

                if (student.Address != null)
                {
                    student.Address.StudentId = student.Id;
                    student.Address.Student = student;
                    await _context.Addresses.AddAsync(student.Address);
                }
                if (student.Identity != null)
                {
                    student.Identity.StudentId = student.Id;
                    student.Identity.Student = student;
                    await _context.Identities.AddAsync(student.Identity);
                }

                await _context.SaveChangesAsync();

                return Result<Student>.Ok(student);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Students_email") == true)
            {
                return Result<Student>.Fail("EMAIL_EXISTS", "Email has alreay existed");
            }
            catch (Exception)
            {
                return Result<Student>.Fail("ADD_FAIL", "Add student failed");
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
                    .Include(s => s.Address)
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
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Students_email") == true)
            {
                return Result<string>.Fail("EMAIL_EXISTS", "Email has alreay existed");
            }
            catch (Exception)
            {
                return Result<string>.Fail("UPDATE_FAILED", "Update student failed");
            }
        }


        //public async Task<bool> IsEmailExistAsync(string email)
        //{
        //    try
        //    {
        //        var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        //        return student != null;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public Task<bool> AddAddressAsync(Address address)
        //{
        //    throw new NotImplementedException();
        //}
    }
}