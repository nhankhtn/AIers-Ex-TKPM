using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Utils
{
    public static class ParseExtensions
    {
        public static Guid ToGuid(this string? value)
        {
            if (value is null)
            {
                return Guid.Empty;
            }
            if (Guid.TryParse(value, out Guid result))
            {
                return result;
            }
            return Guid.Empty;
        }

        public static T ToEnum<T>(this string? value) where T : struct, Enum
        {
            if (value is null)
            {
                return default;
            }
            if (Enum.TryParse<T>(value, out T result))
            {
                return result;
            }
            return default;
        }
    }
}
