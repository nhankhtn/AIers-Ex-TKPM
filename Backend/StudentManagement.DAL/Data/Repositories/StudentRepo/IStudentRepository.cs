﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Models;
namespace StudentManagement.DAL.Data.Repositories.StudentRepo;

public interface IStudentRepository
{
    Task<(IEnumerable<Student> students, int total)> GetAllStudentsAsync(int page, int pageSize, string? key);

    Task<Student?> GetStudentByIdAsync(string studentId);
    Task<IEnumerable<Student?>> GetStudentsByNameAsync(string name);

    Task<bool> AddStudentAsync(Student student);

    Task<bool> UpdateStudentAsync(Student student);

    Task<bool> DeleteStudentAsync(string studentId);

    Task<bool> IsEmailExistAsync(string email);
}