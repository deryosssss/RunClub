using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using RunClubAPI.Middleware;
using System.Threading.Tasks;

namespace RunClubAPI.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "🚨 Exception caught in middleware. Trace ID: {TraceId}, Path: {Path}",
                    context.TraceIdentifier, context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred. Please try again later.",
                detail = _env.IsDevelopment() ? exception.StackTrace : (string?)null, // Use nullable string for 'detail'
                traceId = context.TraceIdentifier
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

// ✅ Extension Method for Cleaner Startup Configuration
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
