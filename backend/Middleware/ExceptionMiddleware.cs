using Microsoft.AspNetCore.Http; // Required for handling HTTP requests
using Microsoft.Extensions.Logging; // Provides logging capabilities
using System;
using System.Net; // For HTTP status codes
using System.Text.Json; // For JSON serialization
using System.Threading.Tasks; // Required for asynchronous operations

namespace RunClubAPI.Middleware
{
    /// <summary>
    /// Middleware for handling unhandled exceptions globally.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; // Represents the next middleware in the pipeline
        private readonly ILogger<ExceptionMiddleware> _logger; // Logger for capturing exceptions

        /// <summary>
        /// Constructor that injects the next middleware delegate and logger.
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware entry point that wraps request processing in a try-catch block.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Passes control to the next middleware
            }
            catch (Exception ex) // Catches any unhandled exceptions
            {
                _logger.LogError(ex, "An unhandled exception occurred."); // Logs the exception
                await HandleExceptionAsync(context, ex); // Handles the exception response
            }
        }

        /// <summary>
        /// Generates a structured JSON response for unhandled exceptions.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; // Set response content type
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500 Internal Server Error

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred. Please try again later.", // Generic message for security
                Details = exception.Message // In production, avoid exposing exception details
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse); // Convert to JSON
            return context.Response.WriteAsync(jsonResponse); // Send response to client
        }
    }
}

/* 
The ExceptionMiddleware is a global error-handling middleware designed to catch unhandled exceptions in an ASP.NET Core application. It ensures that all exceptions are properly logged and returns a structured JSON response instead of crashing the application.

Why Use Middleware for Exception Handling?

Centralized error handling: No need to write try-catch in every controller.
Consistent API responses: Ensures all errors follow a structured JSON format.
Improved debugging: Logs errors for developers while hiding sensitive details from users.
Better security: Prevents exposing stack traces that attackers could exploit.
How It Works?

InvokeAsync(HttpContext context)
It intercepts all HTTP requests and forwards them to the next middleware.
If an exception occurs, it catches it and logs the error.
HandleExceptionAsync(HttpContext context, Exception exception)
Sets the response type as JSON.
Returns HTTP 500 (Internal Server Error) with a generic message to the client.
Logs the actual error for debugging (but in production, details should be hidden).
How to Register This Middleware?


This ensures that all requests go through the exception handler before reaching the controllers.
By implementing ExceptionMiddleware, we ensure that our API remains robust, secure, and user-friendly, preventing unexpected crashes and exposing sensitive information.
*/