using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Unit.Repository
{
    [Collection("ProgramRepository")]

    public class ProgramRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly IProgramRepository _programRepository;

        public ProgramRepositoryTest()
        {
            _context = TestDbContextFactory.Create();
            _programRepository = new ProgramRepository(_context);
        }

        [Fact]
        public async Task GetAllProgramsAsync_ProgramsExist_ReturnsAllPrograms()
        {
            var programs = await _programRepository.GetAllProgramsAsync();
            Assert.NotNull(programs);
            Assert.Equal(3, programs.Count());
        }

        [Fact]
        public async Task GetProgramByIdAsync_ProgramExists_ReturnsProgram()
        {
            var program = await _programRepository.GetProgramByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(program);
            Assert.Equal(TestDbContextFactory.Guid1, program.Id);
        }

        [Fact]
        public async Task GetProgramByIdAsync_ProgramNotExists_ReturnsNull()
        {
            var program = await _programRepository.GetProgramByIdAsync(Guid.NewGuid());
            Assert.Null(program);
        }

        [Fact]
        public async Task GetProgramByNameAsync_ProgramExists_ReturnsProgram()
        {
            var p = await _programRepository.GetProgramByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(p);
            var program = await _programRepository.GetProgramByNameAsync(p.Name);
            Assert.NotNull(program);
            Assert.Equal(p.Name, program.Name);
        }

        [Fact]
        public async Task GetProgramByNameAsync_ProgramNotExists_ReturnsNull()
        {
            var program = await _programRepository.GetProgramByNameAsync("Chuong Trinh X");
            Assert.Null(program);
        }

        [Fact]
        public async Task AddProgramAsync_ProgramNotExist_AddsProgram()
        {
            var program = new Program
            {
                Id = Guid.NewGuid(),
                Name = "Chuong Trinh Moi",
            };

            await _programRepository.AddProgramAsync(program);
            var addedProgram = await _context.Programs.FindAsync(program.Id);
            Assert.NotNull(addedProgram);
            Assert.Equal(program.Name, addedProgram.Name);
        }

        [Fact]
        public async Task UpdateProgramAsync_ProgramExists_UpdatesProgram()
        {
            var program = await _programRepository.GetProgramByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(program);
            program.Name = "Chuong Trinh Cap Nhat";

            var updatedProgram = await _programRepository.UpdateProgramAsync(program);

            Assert.NotNull(updatedProgram);
            Assert.Equal("Chuong Trinh Cap Nhat", updatedProgram.Name);
        }

        [Fact]
        public async Task DeleteProgramAsync_ProgramNotExists_DoesNothing()
        {
            var id = Guid.NewGuid();
            await _programRepository.DeleteProgramAsync(id);
            var program = await _context.Programs.FindAsync(id);
            Assert.Null(program);
        }
    }
}
