﻿using Common;
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

    #region Implicit Operators
    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success);
    }

    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.BadRequest);
    }

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        var message = result.Value.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }
        return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
    }

    public static implicit operator ApiResult(ContentResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.NotFound);
    }
    #endregion
}

public class ApiResult<TData>:ApiResult where TData:class
{
    public ApiResult(bool isSucceeded,ApiResultStatusCode statusCode,TData data,string? message=null):base(isSucceeded,statusCode,message)
    {
        Data = data;
    }
    public TData Data { get; set; }

    #region Implicit Operators

    public static implicit operator ApiResult<TData>(TData data)
        => new ApiResult<TData>(true, ApiResultStatusCode.Success,data);
    

    public static implicit operator ApiResult<TData>(NotFoundResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.NotFound,null);
    

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.NotFound,(TData)result.Value);

    public static implicit operator ApiResult<TData>(OkResult result)

        => new ApiResult<TData>(true, ApiResultStatusCode.Success, null);

    public static implicit operator ApiResult<TData>(OkObjectResult result)
        => new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);

    public static implicit operator ApiResult<TData>(ContentResult result)
        => new ApiResult<TData>(true, ApiResultStatusCode.Success, null, result.Content);

    public static implicit operator ApiResult<TData>(BadRequestResult result)
        => new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        string? message = result.Value.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(e => (string[]) e.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }

        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
    }

    #endregion
}