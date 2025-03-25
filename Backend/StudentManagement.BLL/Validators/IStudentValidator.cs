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
        List<string> NeedValidateProperties { get; }
        bool StudentValidate(string propertyName, StudentDTO value);
    }
}
