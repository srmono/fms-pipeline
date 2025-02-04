using FastEndpoints;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace fleetmanagement.middlware;


public class GlobalExceptionMiddleware {

   // private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger
    ){
        _logger = logger;
    }

    public Task HandleAsync(Exception ex, HttpContext httpContext){

         _logger.LogError(ex, "An unhandled exception occured");

        //Check the expection type and return appropriate status
        var statusCode = HttpStatusCode.InternalServerError; //Default error

        //Custom handling for specific expception (eg. NotFoundException)
        if(ex is KeyNotFoundException){
            statusCode = HttpStatusCode.NotFound;
        } else if(ex is ArgumentException){
            statusCode = HttpStatusCode.BadRequest;
        }

        //set the status code and response content
        httpContext.Response.StatusCode = (int)statusCode;

        return httpContext.Response.WriteAsJsonAsync(new { message = ex.Message });

    }

    //  public async Task InvokeAsync(HttpContext context){
    //     try
    //     {
    //         await _next(context);
    //     }
    //     catch (Exception ex)
    //     {
            
    //         _logger.LogError(ex, "An unhandled exception occured");



    //         //await HandleExceptionAsync(context, ex);
    //     }
    // }
}