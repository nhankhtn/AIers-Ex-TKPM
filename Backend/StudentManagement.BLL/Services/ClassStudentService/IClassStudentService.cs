using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.ClassStudentService
{
    public interface IClassStudentService
    {
        Task<Result<AddStudentToClassDTO>> AddStudentAsync(AddStudentToClassDTO addStudentToClassDTO);

        Task<Result<RegisterCancelationDTO>> RegisterCancelationAsync(RegisterCancelationDTO registerCancelationDTO);

        Task<Result<GetClassStudentsDTO>> GetClassStudentsAsync(int? classId = null, string? studentId = null, int? page = null, int? limit = null);

        Task<Result<GetClassStudentDTO>> UpdateClassStudentAsync(int classId, string studentId, UpdateClassStudentDTO updateClassStudentDTO);
    }
}
