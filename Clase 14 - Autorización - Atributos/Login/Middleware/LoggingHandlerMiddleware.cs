namespace Login.Middleware
{
    /* Clase 11: um middleware es una clase cuyo constructor debe soportar el parámetro RequestDelegate next,
     * y también debe contener el método public async Task InvokeAsync(HttpContext context).
     * 
     * Hay que recordar qu el método InvokeAsync debe invocar al delegado next para continuar 
     * la cadena de middlewares.
     */
    public class LoggingHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingHandlerMiddleware(RequestDelegate next)
        {
            // Clase 11: sintaxis para enganchar el middleware al siguietne eslabón.
            _next = next;
        }

        // Clase 11: Método que implementa la funcionalidad del middleware
        public async Task InvokeAsync(HttpContext context)
        {
            // Clase 11: Antes de procesar el controlador
            Console.WriteLine(">>> Entrando al middleware de logging");

            string id = $"{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort} {context.Request.Method} {context.Request.Path}";

            Console.WriteLine($"{id}");

            try
            {
                // Clase 11: Invocación al controlador o siguiente middleware
                await _next(context);
            } catch (Exception ex)
            {
                // Clase 11: Manejo de errores originados en los controladores.
                Console.WriteLine($"{id} -> {context.Response.StatusCode} {ex?.Message}");

                Console.WriteLine("<<< Saliendo del middleware de logging por exception");

                // Clase 11: Debido a que el error no se lo maneja unicamente se lo registra se vuelve a lanzar.
                throw;
            }

            // Clase 11: Luego de procesar el controlador

            Console.WriteLine($"{id} -> {context.Response.StatusCode}");

            Console.WriteLine("<<< Saliendo del middleware de logging");
        }
    }
}
