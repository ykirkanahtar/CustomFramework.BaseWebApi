using System.Net;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public interface IExceptionStrategy
    {
        string GetReturnMessage(ref string message);

        HttpStatusCode GetHttpStatusCode();
    }
}