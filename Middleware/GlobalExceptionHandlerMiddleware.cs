using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using System.Text.Json;
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
                _logger.LogError(ex, "Exception caught in middleware. Trace ID: {TraceId}, Path: {Path}",
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
                detail = _env.IsDevelopment() ? exception.StackTrace : null,
                traceId = context.TraceIdentifier
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}

/*

The GlobalExceptionHandlerMiddleware is a middleware component in an ASP.NET Core application designed to handle exceptions that occur during request processing. Middleware in ASP.NET Core is a pipeline mechanism where requests pass through multiple layers before reaching their intended endpoint. If an unhandled exception occurs at any stage, it can disrupt the request lifecycle and result in an unstructured or confusing error response. This middleware prevents such issues by intercepting all exceptions, logging them for debugging purposes, and returning a standardized JSON response to the client.

One of the key features of this middleware is its ability to differentiate between development and production environments. If an exception occurs in a development environment, the response includes detailed error messages and stack traces, which help developers diagnose the issue. However, in a production environment, the middleware ensures security by hiding internal details and providing a generic error message to prevent information leakage.

The middleware also logs every exception, along with a unique traceId and the request path, making it easier to track errors in the logs. This enhances debugging and monitoring, especially in distributed systems where identifying issues across multiple services can be challenging.

To integrate this middleware into an ASP.NET Core application, the extension method UseGlobalExceptionHandler() is used within Program.cs. This ensures a clean and reusable setup without modifying the core middleware registration manually. This approach centralizes error handling, improves maintainability, and enhances the overall robustness of the application. */