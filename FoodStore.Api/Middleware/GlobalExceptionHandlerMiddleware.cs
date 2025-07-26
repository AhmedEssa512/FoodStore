using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FoodStore.Contracts.Common;
using FoodStore.Services.Exceptions;

namespace FoodStore.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); 
            }
            catch (NotFoundException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ConflictException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (OperationFailedException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                await WriteJsonErrorResponse(httpContext, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        private async Task WriteJsonErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Fail(message);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
    