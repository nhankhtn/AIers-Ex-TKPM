using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Utils;
using StudentManagement.Domain.Models;
namespace StudentManagement.DAL.Data.Repositories.StudentRepo;

public interface IStudentRepository
{
    Task<Result<(IEnumerable<Student> students, int total)>> GetAllStudentsAsync(int page, int pageSize, string? key);

    Task<Result<Student?>> GetStudentByIdAsync(string studentId);
    Task<Result<IEnumerable<Student?>>> GetStudentsByNameAsync(string name);

    Task<Result<string>> AddStudentAsync(Student student);

    Task<Result<string>> UpdateStudentAsync(Student student);

    Task<Result<string>> DeleteStudentAsync(string studentId);

    //Task<Result<string>> IsEmailExistAsync(string email);

    //Task<Result<string>> AddAddressAsync(Address address);
}