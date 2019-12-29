using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class DefaultError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.AnErrorHasOccured;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.InternalServerError;
        }
    }
}