using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.FacultyService
{
    public interface IFacultyService
    {
        Task<Result<IEnumerable<FacultyDTO>>> GetAllFacultiesAsync();

        Task<Result<FacultyDTO>> ChangeFacultyNameAsync(int id, string newName);

        Task<Result<FacultyDTO>> AddFacultyAsync(string name);
    }
}
