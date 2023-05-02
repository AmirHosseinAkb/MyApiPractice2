using Common;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace WebFramework.Api;

public class ApiResult
{
    public string Message { get; set; }
    public bool IsSucceeded { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }

    public ApiResult(bool isSucceeded,ApiResultStatusCode statusCode,string? message=null)
    {
        IsSucceeded = isSucceeded;
        StatusCode = statusCode;
        Message = message ?? statusCode.ToDisplay();
    }
}

public class ApiResult<TData>:ApiResult where TData:class
{
    public ApiResult(bool isSucceeded,ApiResultStatusCode statusCode,TData data,string? message=null):base(isSucceeded,statusCode,message)
    {
        Data = data;
    }
    public TData Data { get; set; }


    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success,data);
    }

    public static implicit operator ApiResult<TData>(NotFoundResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.NotFound,null);
    

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.NotFound,(TData)result.Value);

    public static implicit operator ApiResult<TData>(BadRequestResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        string message = "";
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(e => (string[]) e.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }

        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
    }
}