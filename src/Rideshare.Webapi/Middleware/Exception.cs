using Newtonsoft.Json;
using Rideshare.Application.Common.Exceptions;
using Rideshare.Application.Exceptions;
using System.Net;

namespace Rideshare.WebApi.Middleware;
public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  public ExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }
  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(httpContext, ex);
    }
  }
  private Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
    string result = JsonConvert.SerializeObject(new ErrorDeatils
    {
      ErrorMessage = exception.Message,
      ErrorType = "Failure"
    });

    switch (exception)
    {
      case BadRequestException badRequestException:
        statusCode = HttpStatusCode.BadRequest;
        result = JsonConvert.SerializeObject(new ErrorDeatils
        {
          ErrorMessage = badRequestException.Message,
          ErrorType = "Bad Request"
        });
        break;
      case ValidationException validationException:
        statusCode = HttpStatusCode.BadRequest;
        result = JsonConvert.SerializeObject(validationException.Errors);
        break;
      case NotFoundException notFoundException:
        statusCode = HttpStatusCode.NotFound;
        result = JsonConvert.SerializeObject(new ErrorDeatils
        {
          ErrorMessage = notFoundException.Message,
          ErrorType = "Not Found"
        });
        break;
      case ConflictException conflictException:
        statusCode = HttpStatusCode.Conflict;
        result = JsonConvert.SerializeObject(new ErrorDeatils
        {
          ErrorMessage = conflictException.Message,
          ErrorType = "Conflict"
        });
        break;
      // case UnauthorizedException unauthorizedException:
      //   statusCode = HttpStatusCode.Unauthorized;
      //   break;
      // case ForbiddenException forbiddenException:
      //   statusCode = HttpStatusCode.Forbidden;
      //   break;
      default:
        break;
    }

    context.Response.StatusCode = (int)statusCode;
    return context.Response.WriteAsync(result);
  }
}

public class ErrorDeatils
{
  public string? ErrorType { get; set; }
  public string? ErrorMessage { get; set; }
}
