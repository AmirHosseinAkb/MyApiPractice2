﻿namespace Common.Exceptions
{
    public class BadRequestException:AppException
    {
        public BadRequestException(string message) : base(ApiResultStatusCode.BadRequest, message)
        {

        }
    }
}
