using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Domain.Utils
{
    public class Result<T>
    {
        public bool Success { get; }
        public T? Data { get; }
        public string? Message { get; }
        public string? ErrorCode { get; }
        public string? ErrorMessage { get; }
        public List<(string errorCode, int index)>? Errors { get; private set; }

        private Result(bool success, T? data, string? message, string? errorCode, string? errorMessage)
        {
            Success = success;
            Data = data;
            Message = message;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Ok(T? data = default, string? message = null)
            => new Result<T>(true, data, message, null, null);
        public static Result<T> Fail(string? errorCode = null, string? errorMessage = null, List<(string errorCode, int index)>? errors = null)
            => new Result<T>(false, default, null, errorCode, errorMessage) { Errors = errors };
        public static Result<T> Multi(T? data, string? errorCode, string? message = null, List<(string errorCode, int index)>? errors = null)
            => new Result<T>(true, data, message, errorCode, null) { Errors = errors };
    }
}
