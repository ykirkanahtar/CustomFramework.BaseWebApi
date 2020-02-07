using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class UnauthorizedAccessError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.UnauthorizedAccessError;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.Forbidden;
        }
    }
}