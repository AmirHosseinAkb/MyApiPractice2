namespace Common.Exceptions
{
    public class BadRequestException:AppException
    {
        public BadRequestException(ApiResultStatusCode statusCode, string message) : base(statusCode, message)
        {

        }
    }
}
