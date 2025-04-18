using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.BLL.DTOs.Class;
using StudentManagement.BLL.DTOs.ClassStudent;
using StudentManagement.BLL.DTOs.Score;
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
        Task<Result<IEnumerable<GetClassStudentDTO>>> AddStudentAsync(string studentId, IEnumerable<string> classIds);

        Task<Result<RegisterCancelationDTO>> RegisterCancelationAsync(RegisterCancelationDTO registerCancelationDTO);

        Task<Result<GetRegisterCancelationHistoryDTO>> GetRegisterCancelationHistoryAsync(int? page, int? limit, string? key);

        Task<Result<GetClassStudentsDTO>> GetClassStudentsAsync(string? classId = null, string? studentId = null, int? page = null, int? limit = null);

        Task<Result<IEnumerable<GetScoreDTO>>> UpdateScoresAsync(string classId, IEnumerable<UpdateScoreDTO> updateScoresDTO);
        Task<Result<IEnumerable<GetScoreDTO>>> GetScoresAsync(string classId);

        Task<Result<StudentTranscriptDTO>> GetStudentTranscriptAsync(string studentId);

        Task<Result<IEnumerable<GetClassDTO>>> GetRegisterableClassesAsync(string studentId);
    }
}
