using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Attributes
{
    public class UniqueConstrainAttribute : Attribute
    {
        public string ErrorMessage { get;}
        public UniqueConstrainAttribute(string errorMessage = "")
        {
            ErrorMessage = errorMessage;
        }
    }
}
