namespace Common.Exceptions
{
    public class NotFoundException:AppException
    {
        public NotFoundException(string message) : base(ApiResultStatusCode.NotFound, message)
        {
        }
    }
}
