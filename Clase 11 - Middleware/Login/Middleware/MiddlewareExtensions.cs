namespace Login.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingHandlerMiddleware>();
        }

        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
