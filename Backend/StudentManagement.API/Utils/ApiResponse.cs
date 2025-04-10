using System.Net;
using System.Text.Json.Serialization;

namespace StudentManagement.API.Utils
{
    /// <summary>
    /// Response from API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {

        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        /// <summary>
        /// Error
        /// </summary>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ApiError? Error { get; set; }

        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ApiError>? Errors { get; set; }

        // Constructor
        public ApiResponse(T? data = default, ApiError? error = null, string? message = null)
        {
            //Status = status;
            //Title = title;
            Data = data;
            Error = error;
            Message = message;
        }

        /// <summary>
        /// Suucess response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse<T> Success(
            string? title = null,
            T? data = default,
            string? message = null)
           => new ApiResponse<T>( data, null, message);

        /// <summary>
        /// BadRequest response
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse<T> BadRequest(
            string? title = null,
            T? data = default,
            ApiError? error = null,
            string? message = null)
            => new ApiResponse<T>(data, error, message);

        /// <summary>
        /// NotFound response
        /// </summary>
        /// <param name="title"></param>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse<T> NotFound(
            string? title = null,
            T? data = default,
            ApiError? error = null,
            string? message = null)
            => new ApiResponse<T>(data, error, message);

        /// <summary>
        /// Unauthorized response
        /// </summary>
        /// <param name="title"></param>
        /// <param name="data"></param>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponse<T> InternalServerError(
            string? title = null,
            T? data = default,
            ApiError? error = null,
            string? message = null)
            => new ApiResponse<T>(data, error, message);

        /// <summary>
        /// MultiStatus response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ApiResponse<T> MultiStatus(
                T? data = default,
                string? message = null,
                List<ApiError>? errors = null
            )
            => new ApiResponse<T>(data, null, message) { Errors = errors };
    }

    // Class ApiError
    public class ApiError
    {
        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Index { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [JsonPropertyName("code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
    }
}