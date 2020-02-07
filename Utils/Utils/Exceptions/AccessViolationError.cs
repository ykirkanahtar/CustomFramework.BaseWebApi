using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class AccessViolationError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.AccessViolationError;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.BadRequest;
        }
    }
}