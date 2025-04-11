using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassRepo
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetClassesAsync(string? courseId = null, int? page = null, int? limit = null);
        Task<Class?> GetClassByIdAsync(int id);
        Task<Class?> AddClassAsync(Class classEntity);
        Task UpdateClassAsync(Class classEntity);
        Task DeleteClassAsync(int id);

    }
}
