using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Utils
{
    public class Result<T>
    {
        public bool Success { get; }
        public T? Data { get; }
        public string? ErrorCode { get; }

        private Result(bool success, T? data, string? errorMessage)
        {
            Success = success;
            Data = data;
            ErrorCode = errorMessage;
        }

        public static Result<T> Ok(T data) => new Result<T>(true, data, null);
        public static Result<T> Fail(string errorMessage) => new Result<T>(false, default, errorMessage);
    }

}
