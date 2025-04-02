using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Unit.Repository
{
    [Collection("StudentRepository")]
    public class StatusRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentStatusRepository _statusRepository;
        private readonly IStudentRepository _studentRepository;

        public StatusRepositoryTest()
        {
            _context = TestDbContextFactory.Create();
            _statusRepository = new StudentStatusRepository(_context);
            _studentRepository = new StudentRepository(_context);
        }

        [Fact]
        public async Task GetAllStatusesAsync_StatusesExist_ReturnsAllStatuses()
        {
            var statuses = await _statusRepository.GetAllStudentStatusesAsync();
            Assert.NotNull(statuses);
            Assert.Equal(2, statuses.Count());
        }

        [Fact]
        public async Task GetStatusByIdAsync_StatusExists_ReturnsStatus()
        {
            var status = await _statusRepository.GetStudentStatusByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(status);
            Assert.Equal(TestDbContextFactory.Guid1, status.Id);
        }

        [Fact]
        public async Task GetStatusByIdAsync_StatusNotExists_ReturnsNull()
        {
            var status = await _statusRepository.GetStudentStatusByIdAsync(Guid.NewGuid());
            Assert.Null(status);
        }

        [Fact]
        public async Task GetStatusByNameAsync_StatusExists_ReturnsStatus()
        {
            var s = await _statusRepository.GetStudentStatusByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(s);
            var status = await _statusRepository.GetStudentStatusByNameAsync(s.Name);
            Assert.NotNull(status);
            Assert.Equal(s.Name, status.Name);
        }

        [Fact]
        public async Task GetStatusByNameAsync_StatusNotExists_ReturnsNull()
        {
            var status = await _statusRepository.GetStudentStatusByNameAsync("abc");
            Assert.Null(status);
        }

        [Fact]
        public async Task AddStatusAsync_StatusNotExist_AddsStatus()
        {
            var status = new StudentStatus
            {
                Id = Guid.NewGuid(),
                Name = "Pending",
            };

            await _statusRepository.AddStudentStatusAsync(status);
            var addedStatus = await _context.StudentStatuses.FindAsync(status.Id);
            Assert.NotNull(addedStatus);
            Assert.Equal(status.Name, addedStatus.Name);
        }

        [Fact]
        public async Task UpdateStatusAsync_StatusExists_UpdatesStatus()
        {
            var status = await _statusRepository.GetStudentStatusByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(status);
            status.Name = "Suspended";

            var updatedStatus = await _statusRepository.UpdateStudentStatusAsync(status);

            Assert.NotNull(updatedStatus);
            Assert.Equal("Suspended", updatedStatus.Name);
        }

        [Fact]
        public async Task DeleteStatusAsync_StatusWithStudents_DeleteStudentsBeforeStatus()
        {
            var status = await _statusRepository.GetStudentStatusByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(status);

            var student = await _studentRepository.GetAllStudentsAsync(1, 10, null, null, status.Name, null);

            foreach (var s in student.students)
            {
                await _studentRepository.DeleteStudentAsync(s.Id);
            }

            await _statusRepository.DeleteStudentStatusAsync(status.Id);
            var deletedStatus = await _context.StudentStatuses.FindAsync(status.Id);
            Assert.Null(deletedStatus);
        }

        [Fact]
        public async Task DeleteStatusAsync_StatusNotExists_DoesNothing()
        {
            var id = Guid.NewGuid();
            await _statusRepository.DeleteStudentStatusAsync(id);
            var status = await _context.StudentStatuses.FindAsync(id);
            Assert.Null(status);
        }
    }
}
