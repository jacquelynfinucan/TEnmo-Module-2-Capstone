using System;

namespace TenmoClient.Exceptions
{
    public class NonSuccessException : Exception
    {
        private const string NON_SUCCESS_MESSAGE = "Error occurred - received non-success response: ";

        // default constructors
        public NonSuccessException() : base() { }
        public NonSuccessException(string message) : base(message) { }
        public NonSuccessException(string message, Exception innerException) : base(message, innerException) { }

        // custom constructors
        public NonSuccessException(int statusCode) : base(NON_SUCCESS_MESSAGE + statusCode) { }

        public NonSuccessException(int statusCode, Exception innerException) : base(NON_SUCCESS_MESSAGE + statusCode, innerException) { }
    }
}
