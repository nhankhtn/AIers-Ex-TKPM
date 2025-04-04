using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Program?> AddProgramAsync(Program program)
        {
            program.Id = Guid.NewGuid();
            await _context.Programs.AddAsync(program);
            await _context.SaveChangesAsync();
            var addedProgram = await _context.Programs.FirstOrDefaultAsync(p => p.Id == program.Id);
            return addedProgram;
        }

        public async Task<IEnumerable<Program>> GetAllProgramsAsync()
        {
            var programs = await _context.Programs.ToListAsync();
            return programs;
        }

        public async Task<Program?> GetProgramByIdAsync(Guid id)
        {
            var program = await _context.Programs.FindAsync(id);
            return program;
        }

        public async Task<Program?> GetProgramByIdAsync(string id) => await GetProgramByIdAsync(id.ToGuid());

        public async Task<Program?> GetProgramByNameAsync(string name)
        {
            var program = await _context.Programs.FirstOrDefaultAsync(p => p.Name == name);
            return program;
        }

        public async Task<Program?> UpdateProgramAsync(Program program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task DeleteProgramAsync(Guid programId)
        {
            var existingProgram = await _context.Programs.FindAsync(programId);
            if (existingProgram is null) return;
            _context.Programs.Remove(existingProgram);
            await _context.SaveChangesAsync();
        }

        private bool IsConnectDatabaseFailed(SqlException ex)
        {
            return ex.Number == 10061;
        }
    }
}