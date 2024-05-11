using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using SendGrid.Helpers.Errors.Model;

namespace Backend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"--- Exception Handling Requests: {context.Request.Method} {context.Request.Path} ---");

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled exception: {ex}");

                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                _logger.LogInformation($"--- Exception Handling Requests Ends : Response status {context.Response.StatusCode} ---");
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var responseCode = StatusCodes.Status500InternalServerError;
            var message = "Internal Server Error from the middleware";

            switch (exception)
            {
                case NotFoundException notFoundException:
                    responseCode = StatusCodes.Status404NotFound;
                    message = notFoundException.Message;
                    break;

                case ValidationException validationException:
                    responseCode = StatusCodes.Status400BadRequest;
                    message = validationException.Message;
                    break;

                case UnauthorizedException unauthorizedException:
                    responseCode = StatusCodes.Status401Unauthorized;
                    message = unauthorizedException.Message;
                    break;

                case ForbiddenException forbiddenException:
                    responseCode = StatusCodes.Status403Forbidden;
                    message = forbiddenException.Message;
                    break;
                
                case InvalidOperationException invalidOperationException:
                    responseCode = StatusCodes.Status400BadRequest;
                    message = invalidOperationException.Message;
                    break;

                default:
                    if (exception is ApplicationException)
                    {
                        responseCode = StatusCodes.Status400BadRequest;
                        message = exception.Message;
                    }

                    break;
            }

            context.Response.StatusCode = responseCode;

            var response = new
            {
                StatusCode = responseCode,
                Message = message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}