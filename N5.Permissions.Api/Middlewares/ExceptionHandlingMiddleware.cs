// *? n5-reto-tecnico-api/N5.Permissions.Api/Middlewares/ExceptionHandlingMiddleware.cs

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace N5.Permissions.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string title = "An error occurred while processing your request.";

            // Mapear excepciones específicas utilizando patrones
            switch (exception)
            {
                case ArgumentNullException _:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    break;
                case ValidationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    break;
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    break;
                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized";
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Not Found";
                    break;
                case NotImplementedException _:
                    statusCode = HttpStatusCode.NotImplemented;
                    title = "Not Implemented";
                    break;
            }

            logger.LogError(exception, "Unhandled exception occurred.");

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(problemDetails, options);
            return context.Response.WriteAsync(result);
        }
    }
}
