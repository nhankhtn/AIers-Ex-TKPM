using StudentManagement.BLL.DTOs;
using StudentManagement.Domain.Utils;
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

        Task<Result<FacultyDTO>> UpdateFacultyAsync(int id, FacultyDTO facultyDTO);

        Task<Result<FacultyDTO>> AddFacultyAsync(FacultyDTO facultyDTO);
    }
}
