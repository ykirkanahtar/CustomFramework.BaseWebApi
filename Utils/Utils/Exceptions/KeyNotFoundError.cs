using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class KeyNotFoundError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.NotFoundError;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.NotFound;
        }
    }
}