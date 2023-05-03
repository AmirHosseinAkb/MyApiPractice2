namespace Common.Exceptions
{
    public class NotFoundException:AppException
    {
        public NotFoundException(ApiResultStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
