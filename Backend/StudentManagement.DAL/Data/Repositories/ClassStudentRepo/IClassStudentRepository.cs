using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassStudentRepo
{
    public interface IClassStudentRepository
    {
        Task<IEnumerable<ClassStudent>> GetAllClassStudentsAsync();
        Task<ClassStudent?> GetClassStudentByIdAsync(int classId, string studentId);
        Task AddClassStudentAsync(ClassStudent classStudent);
        Task UpdateClassStudentAsync(ClassStudent classStudent);
        Task DeleteClassStudentAsync(int classId, string studentId);
    }
}
