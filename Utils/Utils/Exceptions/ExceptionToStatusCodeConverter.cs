using System;
using System.Net;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public static class ExceptionToStatusCodeConverter
    {
        public static HttpStatusCode ExceptionToStatusCode(this Exception exception)
        {
            return new ExceptionOperation(exception).GetHttpStatusCode();
        }
    }
}