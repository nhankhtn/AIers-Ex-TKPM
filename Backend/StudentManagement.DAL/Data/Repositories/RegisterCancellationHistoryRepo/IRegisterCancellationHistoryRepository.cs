using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo
{
    public interface IRegisterCancellationHistoryRepository
    {
        Task<ICollection<RegisterCancellationHistory>> GetAllAsync(int? page = null, int? limit = null, string? key = null);
        Task<RegisterCancellationHistory?> GetByIdAsync(int id);
        Task AddAsync(RegisterCancellationHistory registerCancellationHistory);
        Task UpdateAsync(RegisterCancellationHistory registerCancellationHistory);
        Task DeleteAsync(int id);
    }
}
