using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Models;
namespace StudentManagement.DAL.Data.Repositories.StudentRepo;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
}
