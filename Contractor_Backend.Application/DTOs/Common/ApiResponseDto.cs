namespace Contractor_Backend.Application.DTOs.Common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
