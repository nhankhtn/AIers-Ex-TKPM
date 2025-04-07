using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<RegisterCancellationHistory>> GetAllAsync()
        {
            var registerCancellationHistories = await _context.RegisterCancellationHistories.ToListAsync();
            return registerCancellationHistories;
        }

        public async Task<RegisterCancellationHistory?> GetByIdAsync(int id)
        {
            var registerCancellationHistory = await _context.RegisterCancellationHistories.FindAsync(id);
            return registerCancellationHistory;
        }

        public Task UpdateAsync(RegisterCancellationHistory registerCancellationHistory)
        {
            var existingRegisterCancellationHistory = _context.RegisterCancellationHistories.Find(registerCancellationHistory.Id);
            if (existingRegisterCancellationHistory == null)
            {
                throw new ArgumentException("Register Cancellation History Not Found");
            }
            existingRegisterCancellationHistory.ClassId = registerCancellationHistory.ClassId;
            existingRegisterCancellationHistory.StudentId = registerCancellationHistory.StudentId;
            existingRegisterCancellationHistory.Time = registerCancellationHistory.Time;
            _context.RegisterCancellationHistories.Update(existingRegisterCancellationHistory);
            return _context.SaveChangesAsync();
        }
    }
}
