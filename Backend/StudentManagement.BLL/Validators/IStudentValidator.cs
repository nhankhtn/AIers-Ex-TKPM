using StudentManagement.BLL.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Validators
{
    public interface IStudentValidator
    {
        (bool IsValid, string ErrorCode, string ErrorMessage) StudentValidate(StudentDTO value, bool isUpdate = false);
    }
}
