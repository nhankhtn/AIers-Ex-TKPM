using StudentManagement.BLL.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Validators
{
    public interface IUserValidator
    {
        List<string> NeedValidateProperties { get; }
        Task<bool> StudentValidate(string propertyName, StudentDTO value);
    }
}
