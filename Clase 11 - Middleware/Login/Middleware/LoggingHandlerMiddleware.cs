namespace Login.Middleware
{
    public class LoggingHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(">>> Entrando al middleware de logging");

            string id = $"{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} {context.Request.Method} {context.Request.Path}";

            Console.WriteLine($"{id}");

            try
            {
                await _next(context);
            } catch (Exception ex)
            {
                Console.WriteLine($"{id} -> {context.Response.StatusCode} {ex?.Message}");

                Console.WriteLine("<<< Saliendo del middleware de logging por exception");

                throw;
            }

            Console.WriteLine($"{id} -> {context.Response.StatusCode}");

            Console.WriteLine("<<< Saliendo del middleware de logging");
        }
    }
}
