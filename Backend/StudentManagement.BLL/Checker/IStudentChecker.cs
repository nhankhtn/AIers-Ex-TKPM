using StudentManagement.BLL.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Checker
{
    public interface IStudentChecker
    {
        Task<(bool IsValid, string ErrorCode, string ErrorMessage)> StudentCheckAsync(StudentDTO studentDTO, bool isUpdate = false);
    }
}
