using Microsoft.AspNetCore.Http; // Required for handling HTTP context in middleware.
using Microsoft.Extensions.Logging; // Provides logging capabilities.
using System;
using System.Net; // Contains HTTP status codes.
using System.Text.Json; // Used for JSON serialization.
using RunClubAPI.Middleware;
using System.Threading.Tasks; // Supports asynchronous programming.

namespace RunClubAPI.Middleware
{
    /// <summary>
    /// Middleware for global exception handling. It catches unhandled exceptions,
    /// logs them, and returns a structured error response to the client.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next; // The next middleware in the pipeline.
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger; // Logger for capturing errors.
        private readonly IWebHostEnvironment _env; // Provides environment information (Development, Production, etc.).

        /// <summary>
        /// Constructor to initialize middleware dependencies.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">Logger instance for error logging.</param>
        /// <param name="env">Environment details (used to differentiate development and production modes).</param>
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Middleware execution method. It tries to execute the next middleware
        /// in the pipeline and handles any exceptions that occur.
        /// </summary>
        /// <param name="context">The HTTP context of the request.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Pass the request to the next middleware.
            }
            catch (Exception ex)
            {
                // Logs error details including the request path and a unique trace identifier.
                _logger.LogError(ex, "Exception caught in middleware. Trace ID: {TraceId}, Path: {Path}",
                    context.TraceIdentifier, context.Request.Path);

                // Handles the exception and sends a structured response to the client.
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions by returning a JSON response with relevant error details.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="exception">The exception that was thrown.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; // Set response type to JSON.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Default status code for unhandled exceptions.

            // Constructs the response object with error details.
            var response = new
            {
                message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred. Please try again later.",
                detail = _env.IsDevelopment() ? exception.StackTrace : (string?)null, // Show stack trace only in development.
                traceId = context.TraceIdentifier // Unique identifier for tracing requests in logs.
            };

            // Serializes the response object to JSON format.
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Sends the JSON response to the client.
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

// Extension method for cleaner middleware configuration.
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Extension method to add global exception handling middleware to the application pipeline.
    /// </summary>
    /// <param name="app">Application builder instance.</param>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}

/*

The GlobalExceptionHandlerMiddleware is a middleware component in an ASP.NET Core application designed to handle exceptions that occur during request processing. Middleware in ASP.NET Core is a pipeline mechanism where requests pass through multiple layers before reaching their intended endpoint. If an unhandled exception occurs at any stage, it can disrupt the request lifecycle and result in an unstructured or confusing error response. This middleware prevents such issues by intercepting all exceptions, logging them for debugging purposes, and returning a standardized JSON response to the client.

One of the key features of this middleware is its ability to differentiate between development and production environments. If an exception occurs in a development environment, the response includes detailed error messages and stack traces, which help developers diagnose the issue. However, in a production environment, the middleware ensures security by hiding internal details and providing a generic error message to prevent information leakage.

The middleware also logs every exception, along with a unique traceId and the request path, making it easier to track errors in the logs. This enhances debugging and monitoring, especially in distributed systems where identifying issues across multiple services can be challenging.

To integrate this middleware into an ASP.NET Core application, the extension method UseGlobalExceptionHandler() is used within Program.cs. This ensures a clean and reusable setup without modifying the core middleware registration manually. This approach centralizes error handling, improves maintainability, and enhances the overall robustness of the application. */