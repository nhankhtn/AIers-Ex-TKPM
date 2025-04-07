using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassRepo
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class?> GetClassByIdAsync(int id);
        Task AddClassAsync(Class classEntity);
        Task UpdateClassAsync(Class classEntity);
        Task DeleteClassAsync(int id);
    }
}
