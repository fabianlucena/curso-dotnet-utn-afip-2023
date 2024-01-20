namespace Login.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; private set; } = 500;

        public HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
