using System.Net;
using CustomFramework.BaseWebApi.Utils.Constants;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class DuplicateNameError : IExceptionStrategy
    {
        public string GetReturnMessage(ref string message)
        {
            return DefaultResponseMessages.RecordExistsError;
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return HttpStatusCode.BadRequest;
        }
    }
}