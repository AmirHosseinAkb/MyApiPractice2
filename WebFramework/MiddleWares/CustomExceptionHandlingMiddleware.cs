using System.Net;
using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
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

            apiResultStatusCode = exception.ApiStatusCode;
            httpStatusCode = exception.HttpStatusCode;
            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string?>()
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if (exception.InnerException != null)
                {
                    dic.Add("InnerException.Exception", exception.InnerException.Message);
                    dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace?.ToString());
                }

                if (exception.AdditionalData != null)
                    dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));
                message = JsonConvert.SerializeObject(dic);
            }
            else
                message = exception.Message;

            await WriteToResponseAsync();
        }

        catch (UnauthorizedAccessException exception)
        {
            _logger.LogError(exception, exception.Message);
            SetUnAuthorizedAccessException(exception);
            await WriteToResponseAsync();
        }
        catch (SecurityTokenExpiredException exception)
        {
            _logger.LogError(exception, exception.Message);
            SetUnAuthorizedAccessException(exception);
            await WriteToResponseAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,exception.Message);
            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string?>()
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if (exception.InnerException != null)
                {
                    dic.Add("InnerException.Exception", exception.InnerException.Message);
                    dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                }

                message = JsonConvert.SerializeObject(dic);
            }

            await WriteToResponseAsync();
        }


        async Task WriteToResponseAsync()
        {
            if (httpContext.Response.HasStarted)
                throw new InvalidOperationException("The Operation Of Sending Response Has Started");
            var apiResult = new ApiResult(false, apiResultStatusCode, message);
            var json = JsonConvert.SerializeObject(apiResult);

            httpContext.Response.ContentType="application/json";
            httpContext.Response.StatusCode = (int) httpStatusCode;

            await httpContext.Response.WriteAsync(json);
        }

        void SetUnAuthorizedAccessException(Exception exception)
        {
            apiResultStatusCode = ApiResultStatusCode.UnAuthorized;
            httpStatusCode = HttpStatusCode.Unauthorized;

            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>()
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if(exception is SecurityTokenExpiredException securityTokenExpiredException)
                    dic.Add("Expires",securityTokenExpiredException.Expires.ToString());
                message = JsonConvert.SerializeObject(dic);
            }
        }
    }
    
}