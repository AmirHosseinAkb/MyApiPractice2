namespace Common.Exceptions;

public class AppException:Exception
{
    public ApiResultStatusCode StatusCode { get; set; }

    public AppException(ApiResultStatusCode statusCode,string message):base(message)
    {
        StatusCode = statusCode;
    }
}