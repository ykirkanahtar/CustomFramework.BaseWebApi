using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Security.Authentication;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public class ExceptionOperation : IExceptionStrategy
    {
        private readonly Exception _exception;
        private IExceptionStrategy _exceptionStrategy;

        public ExceptionOperation(Exception exception)
        {
            _exception = exception;
            SelectExceptionStrategy();
        }

        public string GetReturnMessage(ref string message)
        {
            return _exceptionStrategy.GetReturnMessage(ref message);
        }

        public HttpStatusCode GetHttpStatusCode()
        {
            return _exceptionStrategy.GetHttpStatusCode();
        }

        private void SelectExceptionStrategy()
        {
            switch (_exception)
            {
                case AccessViolationException _:
                    _exceptionStrategy = new AccessViolationError();
                    break;
                case DuplicateNameException _:
                    _exceptionStrategy = new DuplicateNameError();
                    break;
                case KeyNotFoundException _:
                    _exceptionStrategy = new KeyNotFoundError();
                    break;
                case ArgumentException _:
                    _exceptionStrategy = new ArgumentError();
                    break;
                case AuthenticationException _:
                    _exceptionStrategy = new AuthenticationError();
                    break;
                case UnauthorizedAccessException _:
                    _exceptionStrategy = new UnauthorizedAccessError();
                    break;
                default:
                    _exceptionStrategy = new DefaultError();
                    break;
            }
        }

    }
}