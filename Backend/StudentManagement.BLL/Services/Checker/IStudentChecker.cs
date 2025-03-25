using StudentManagement.BLL.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.Checker
{
    public interface IStudentChecker
    {
        List<string> NeedCheckedProperties { get; }
        Task<(bool Result, string ErrorCode)> StudentChecked(string propertyName, StudentDTO value, bool isUpdate = false);
    }
}
