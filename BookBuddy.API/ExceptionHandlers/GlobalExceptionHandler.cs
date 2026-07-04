using BookBuddy.API.Exceptions;
using BookBuddy.API.Models.Responses;
using Microsoft.AspNetCore.Diagnostics;
namespace BookBuddy.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }


        //This below One method is Heart of the entire Global Exception Handling.
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            //logic flow :
            /* Exception occured?
                 |          |
               Yes          No
                 │          │
                 ▼          ▼
          What type is it?  complete the request              
        │
        ├── NotFoundException → 404
        │
        ├── BadRequestException → 400
        │
        ├── ConflictException → 409
        │
        └── Anything else → 500
            */

            // In production when Exception occurs, we logs it.

            //Log the Exception
            _logger.LogError(exception, exception.Message);

            //Determine the Status Code
            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                ConflictException => StatusCodes.Status409Conflict,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            // Set the HTTP Response Status Code
            httpContext.Response.StatusCode = statusCode;


            // Create the standardized error response
            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = statusCode == StatusCodes.Status500InternalServerError ? "An unexpected error occurred." : exception.Message
            };

            // Write the error response as JSON
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            // Indicate that the exception has been handled
            return true;
        }
    }
}
