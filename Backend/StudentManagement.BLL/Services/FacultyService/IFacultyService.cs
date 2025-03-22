using StudentManagement.BLL.DTOs.Faculty;
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

        Task<Result<FacultyDTO>> UpdateFacultyAsync(string id, FacultyDTO facultyDTO);

        Task<Result<FacultyDTO?>> AddFacultyAsync(FacultyDTO facultyDTO);

        Task<Result<string>> DeleteFacultyAsync(string id); 
    }
}
