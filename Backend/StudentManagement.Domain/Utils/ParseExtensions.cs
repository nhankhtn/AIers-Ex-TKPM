using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Utils
{
    public static class ParseExtensions
    {
        public static Guid ToGuid(this string value)
        {
            if (Guid.TryParse(value, out Guid result))
            {
                return result;
            }
            return Guid.Empty;
        }
    }
}
