using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ProgramRepo
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProgramAsync(Program program)
        {
            try
            {
                await _context.Programs.AddAsync(program);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Program>> GetAllProgramsAsync()
        {
            try
            {
                var programs = await _context.Programs.ToListAsync();
                return programs;
            }
            catch (Exception)
            {
                return new List<Program>();
            }
        }

        public async Task<Program?> GetProgramByIdAsync(int id)
        {
            try
            {
                var program = await _context.Programs.FindAsync(id);
                return program;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Program?> GetProgramByNameAsync(string name)
        {
            try
            {
                var program = await _context.Programs.FirstOrDefaultAsync(p => p.Name == name);
                return program;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateProgramAsync(Program program)
        {
            try
            {
                var existingProgram = await _context.Programs.FindAsync(program.Id);
                if (existingProgram == null) return false;

                _context.Entry(existingProgram).CurrentValues.SetValues(program);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
