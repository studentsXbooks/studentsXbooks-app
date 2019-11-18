using System;

namespace sXb_service.Exceptions
{
    public class BookApiException : Exception
    {
        public BookApiException()
        {

        }

        public BookApiException(string message) : base(message)
        {

        }
    }
}
