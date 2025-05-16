using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
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
    [Collection("FacultyRepository")]

    public class FacultyRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStudentRepository _studentRepository;

        public FacultyRepositoryTest()
        {
            _context = TestDbContextFactory.Create();
            _facultyRepository = new FacultyRepository(_context);
            _studentRepository = new StudentRepository(_context);
        }

        [Fact]
        public async Task GetAllFacultiesAsync_FacultiesExist_ReturnsAllFaculties()
        {
            var faculties = await _facultyRepository.GetAllFacultiesAsync();
            Assert.NotNull(faculties);
            Assert.Equal(3, faculties.Count());
        }

        [Fact]
        public async Task GetFacultyByIdAsync_FacultyExists_ReturnsFaculty()
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(TestDbContextFactory.Guid1);
            Assert.NotNull(faculty);
            Assert.Equal(TestDbContextFactory.Guid1, faculty.Id);
        }

        [Fact]
        public async Task GetFacultyByIdAsync_FacultyNotExists_ReturnsNull()
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(Guid.NewGuid());
            Assert.Null(faculty);
        }

        [Fact]
        public async Task GetFacultyByNameAsync_FacultyExists_ReturnsFaculty()
        {
            var f = await _facultyRepository.GetFacultyByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(f);
            var faculty = await _facultyRepository.GetFacultyByNameAsync(f.Name);
            Assert.NotNull(faculty);
            Assert.Equal(f.Name, faculty.Name);
        }

        [Fact]
        public async Task GetFacultyByNameAsync_FacultyNotExists_ReturnsNull()
        {
            var faculty = await _facultyRepository.GetFacultyByNameAsync("Khoa Xã Hội");
            Assert.Null(faculty);
        }

        [Fact]
        public async Task AddFacultyAsync_FacultyNotExist_AddsFaculty()
        {
            var faculty = new Faculty
            {
                Id = Guid.NewGuid(),
                Name = "Khoa Hoa Hoc",
            };

            await _facultyRepository.AddFacultyAsync(faculty);
            var addedFaculty = await _context.Faculties.FindAsync(faculty.Id);
            Assert.NotNull(addedFaculty);
            Assert.Equal(faculty.Name, addedFaculty.Name);
        }

        [Fact]
        public async Task UpdateFacultyAsync_FacultyExists_UpdatesFaculty()
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(TestDbContextFactory.Guid1.ToString());
            Assert.NotNull(faculty);
            faculty.Name = "Khoa Kinh Tế";

            var updatedFaculty = await _facultyRepository.UpdateFacultyAsync(faculty);

            Assert.NotNull(updatedFaculty);
            Assert.Equal("Khoa Kinh Tế", updatedFaculty.Name);
        }

        //[Fact]
        //public async Task DeleteFacultyAsync_FacultyWithStudents_DeleteStudentsBeforeFaculty()
        //{
        //    var faculty = await _facultyRepository.GetFacultyByIdAsync(TestDbContextFactory.Guid1.ToString());
        //    Assert.NotNull(faculty);

        //    var student = await _studentRepository.GetAllStudentsAsync(1, 10, faculty.Name, null, null, null);
            
        //    foreach(var s in student.students)
        //    {
        //        await _studentRepository.DeleteStudentAsync(s.Id);
        //    }

        //    await _facultyRepository.DeleteFacultyAsync(faculty.Id);
        //    var deletedFaculty = await _context.Faculties.FindAsync(faculty.Id);
        //    Assert.Null(deletedFaculty);
        //}

        [Fact]
        public async Task DeleteFacultyAsync_FacultyNotExists_DoesNothing()
        {
            var id = Guid.NewGuid();
            await _facultyRepository.DeleteFacultyAsync(id);
            var faculty = await _context.Faculties.FindAsync(id);
            Assert.Null(faculty);
        }
    }
}