using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.RegisterCancellationHistoryRepo
{
    public class RegisterCancellationHistoryRepository : IRegisterCancellationHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public RegisterCancellationHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RegisterCancellationHistory registerCancellationHistory)
        {
           _context.RegisterCancellationHistories.Add(registerCancellationHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var registerCancellationHistory = await _context.RegisterCancellationHistories.FindAsync(id);
            if (registerCancellationHistory == null)
            {
                throw new ArgumentException("Register Cancellation History Not Found");
            }
            _context.RegisterCancellationHistories.Remove(registerCancellationHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<RegisterCancellationHistory>> GetAllAsync(int? page, int? limit, string? key)
        {
            var query = _context.RegisterCancellationHistories.AsQueryable();
            if (key != null)
            {
                query = query.Where(x => x.StudentId.Contains(key) || x.ClassId.Contains(key));
            }

            if (page != null && limit != null)
            {
                query = query.Skip((page.Value - 1) * limit.Value).Take(limit.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<RegisterCancellationHistory?> GetByIdAsync(int id)
        {
            var registerCancellationHistory = await _context.RegisterCancellationHistories.FindAsync(id);
            return registerCancellationHistory;
        }

        public Task UpdateAsync(RegisterCancellationHistory registerCancellationHistory)
        {
            _context.RegisterCancellationHistories.Update(registerCancellationHistory);
            return _context.SaveChangesAsync();
        }
    }
}
