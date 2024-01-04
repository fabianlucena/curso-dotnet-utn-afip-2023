namespace Login.Middleware
{
    public static class MiddlewareExtensions
    {
        /* Clase 11: En esta sección se definen extensiones para IApplicationBuilder
         * de manera que se puedan usar como comandos haciendo más claro la lectura
         * del código y el uso de los middlewares.
         */
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
