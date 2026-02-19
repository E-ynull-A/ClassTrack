using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Utilities;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Diagnostics;

namespace ClassTrack.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Error occured {Message}", exception.Message);
            IDictionary<string, string[]> errors = null;


            var (statusCode, message) = exception switch
            {
                BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),
                BusinessLogicException => (StatusCodes.Status400BadRequest, exception.Message),
                ConflictException => (StatusCodes.Status409Conflict, exception.Message),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                InternalServerException => (StatusCodes.Status500InternalServerError, exception.Message),
                _ => (StatusCodes.Status500InternalServerError,"")
            };

            await httpContext.Response
            .WriteAsJsonAsync(new ErrorResponseDTO
                    (statusCode, message, exception.StackTrace));

            return true;
        }
    }
}
