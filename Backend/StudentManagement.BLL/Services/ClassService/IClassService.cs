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
        Task<Result<GetClassDTO>> AddClassAsync(AddClassDTO addClassDTO);

        Task<Result<GetClassDTO>> GetClassAsync(string classId);

        Task<Result<GetClassesDTO>> GetClassesAsync(string? classId = null, int? semester = null, int? year = null, int? page = null, int? limit = null); 

        Task<Result<GetClassDTO>> DeleteClassAsync(string classId);

        Task<Result<GetClassDTO>> UpdateClassAsync(string classId, UpdateClassDTO updateClassDTO);
    }
}
