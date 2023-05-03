using System.Net;
using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using WebFramework.Api;

namespace WebFramework.MiddleWares;

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder  app)
    {
        return app.UseMiddleware<CustomExceptionHandlingMiddleware>();
    }
}
public class CustomExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
    private readonly IHostingEnvironment _env;
    public CustomExceptionHandlingMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlingMiddleware> logger,IHostingEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        string? message = null;

        var apiResultStatusCode = ApiResultStatusCode.ServerError;
        var httpStatusCode = HttpStatusCode.InternalServerError;

        try
        {
            await _next(httpContext);
        }
        catch (AppException exception)
        {
            _logger.LogError(exception, exception.Message);
            if (_env.IsDevelopment())
            {

            }
            else
            {

            }
                var apiResult = new ApiResult(false, exception.StatusCode, exception.Message);
            var json = JsonConvert.SerializeObject(apiResult);
            httpContext.Response.ContentType="application/json";
            await httpContext.Response.WriteAsync(json);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            var apiresult = new ApiResult(false, ApiResultStatusCode.ServerError, "خطایی رخ داده است");
            var json = JsonConvert.SerializeObject(apiresult);
            httpContext.Response.ContentType="application/json";
            await httpContext.Response.WriteAsync(json);
        }
    }
    
}