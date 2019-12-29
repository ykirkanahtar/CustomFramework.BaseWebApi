using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class AuthenticationError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.LoginError;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.Unauthorized;
        }
    }
}