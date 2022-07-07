using Forum.Application.Exceptions;
using Forum.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Forum.Infrastructure.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (NotFoundException ex)
            {
                this._logger.LogError($"Entity not found: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.NotFound);
            }
            catch (AlreadyExistsException ex)
            {
                this._logger.LogError($"Entity already exists: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.BadRequest);
            }
            catch (InvalidDataException ex)
            {
                this._logger.LogError($"Invalid data: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var message = exception switch
            {
                NotFoundException => $"{exception.Message} Refresh the page and try again.",
                AlreadyExistsException => $"{exception.Message} Change properties or delete existing object.",
                InvalidDataException => $"{exception.Message} Change properties and try again.",
                _ => "Internal Server Error",
            };

            await context.Response.WriteAsync(new ErrorDetails(statusCode, message).ToString());
        }
    }
}
