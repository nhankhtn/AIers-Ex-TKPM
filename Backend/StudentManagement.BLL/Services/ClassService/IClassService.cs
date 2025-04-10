using StudentManagement.BLL.DTOs.Class;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassService
{
    public interface IClassService
    {
        Task<Result<AddClassDTO>> AddClassAsync(AddClassDTO addClassDTO);

        Task<Result<GetClassDTO>> GetClassAsync(int classId);

        Task<Result<IEnumerable<GetClassDTO>>> GetClassesAsync(string? course = null); 

        Task<Result<GetClassDTO>> DeleteClassAsync(int classId);

        Task<Result<GetClassDTO>> UpdateClassAsync(int classId, UpdateClassDTO updateClassDTO);
    }
}
