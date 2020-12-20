using System;

namespace Blog.Service.Identity.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public string Message { get; }
        public ApplicationException(string message) : base(message)
        {
            Message = message;
        }
    }
}
