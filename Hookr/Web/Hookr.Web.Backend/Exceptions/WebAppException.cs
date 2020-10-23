using System;

namespace Hookr.Web.Backend.Exceptions
{
    public class WebAppException : Exception
    {
        public WebAppException(string message) : base(message)
        {
        }
    }
}