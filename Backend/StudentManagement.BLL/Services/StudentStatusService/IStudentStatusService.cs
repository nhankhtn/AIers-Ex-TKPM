using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentStatusService
{
    public interface IStudentStatusService
    {
        Task<Result<IEnumerable<StudentStatusDTO>>> GetAllStudentStatusAsync();

        Task<Result<StudentStatusDTO>> ChangeStudentStatusNameAsync(int id, string newName);

        Task<Result<StudentStatusDTO>> AddStudentStatusAsync(string name);
    }
}
