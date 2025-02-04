using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using fleetmanagement.dtos;
using FastEndpoints;

namespace fleetmanagement.middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationErrorFaulureException ex)
            {
                // Log the validation errors
                var validationErrors = ex.Errors.Select(e => new
                {
                    Field = e.FieldName,
                    Message = e.ErrorMessage
                });

                var response = new
                {
                    Status = "Bad Request",
                    Code = (int)HttpStatusCode.BadRequest,
                    Reason = ex.Message,
                    Errors = validationErrors
                };

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Unhandled exception: {ex.Message}");

                var response = new
                {
                    Status = "Internal Server Error!",
                    Code = (int)HttpStatusCode.InternalServerError,
                    Reason = ex.Message,
                    Note = "See application log for stack trace."
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}


// public class CustomExceptionHandlerMiddleware
// {
//     private readonly RequestDelegate _next;

//     public CustomExceptionHandlerMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     public async Task Invoke(HttpContext context)
//     {
//         try
//         {
//             await _next(context);
//         }
//         catch (ValidationErrorFaulureException ex)
//         {
//             context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//             context.Response.ContentType = "application/json";

//             var response = new
//             {
//                 status = "Bad Request",
//                 code = 400,
//                 reason = "Validation failed",
//                 errors = ex.Failures.Select(f => new { field = f.FieldName, message = f.ErrorMessage })
//             };

//             await context.Response.WriteAsJsonAsync(response);
//         }
//         catch (Exception ex)
//         {
//             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//             context.Response.ContentType = "application/json";

//             var response = new
//             {
//                 status = "Internal Server Error",
//                 code = 500,
//                 reason = ex.Message
//             };

//             await context.Response.WriteAsJsonAsync(response);
//         }
//     }
// }


// public class CustomExceptionHandlerMiddleware
// {
//     private readonly RequestDelegate _next;

//     public CustomExceptionHandlerMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             await _next(context); // Let the next middleware process the request
//         }
//         catch (Exception ex)
//         {
//             // Catch any unhandled exception and return a 500 response
//             await HandleExceptionAsync(context, ex);
//         }
//     }

//     private static Task HandleExceptionAsync(HttpContext context, Exception exception)
//     {
//         context.Response.ContentType = "application/json";
//         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//         // You can log the exception here if needed (use a logger)

//         // Return a generic error message to the client
//         var response = new ValidationFailure(
//             "Internal server error occurred.",  // Pass the message
//             exception.Message  // Include exception details as the details
//         );

//         var jsonResponse = JsonSerializer.Serialize(response);
//         return context.Response.WriteAsync(jsonResponse);
//     }
// }


