using Contractor_Backend.Application.Services.Common;
using System.Text.Json;

namespace Contractor_Backend.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = context.TraceIdentifier;
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);

            var response = ApiResponseFactory.Failure<object>(
                message: "An unexpected error occurred.",
                errors: new List<string> { exception.Message },
                statusCode: StatusCodes.Status500InternalServerError,
                context: context
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
