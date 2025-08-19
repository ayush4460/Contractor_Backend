using Contractor_Backend.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Contractor_Backend.Application.Services.Common
{
    public static class ApiResponseFactory
    {
        private static string GetTraceId(HttpContext? context)
        {
            return context?.TraceIdentifier ?? Activity.Current?.Id ?? Guid.NewGuid().ToString();
        }

        public static ApiResponseDto<T> Success<T>(T data, string message = "Success", HttpContext? context = null)
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TraceId = GetTraceId(context),
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ApiResponseDto<T> Failure<T>(string message, List<string>? errors = null, int statusCode = StatusCodes.Status500InternalServerError, HttpContext? context = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                TraceId = GetTraceId(context),
                StatusCode = statusCode
            };
        }

        public static ApiResponseDto<T> ValidationError<T>(List<string> errors, HttpContext? context = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors,
                TraceId = GetTraceId(context),
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
