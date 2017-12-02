using System;

namespace GalconServer.Model
{
    public class ErrorResponse
    {
        public ErrorResponse(Exception exception, string message)
        {
            Exception = exception.Message;
            Message = message;
        }

        public string Exception { get; }
        public string Message { get; }
    }
}