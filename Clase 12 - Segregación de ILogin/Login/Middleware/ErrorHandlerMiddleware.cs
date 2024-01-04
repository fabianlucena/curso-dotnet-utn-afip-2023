using Logic.Session;

namespace Login.Middleware
{
    /* Clase 11: um middleware es una clase cuyo constructor debe soportar el parámetro RequestDelegate next,
     * y también debe contener el método public async Task InvokeAsync(HttpContext context).
     * 
     * Hay que recordar qu el método InvokeAsync debe invocar al delegado next para continuar 
     * la cadena de middlewares.
     */
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            // Clase 11: sintaxis para enganchar el middleware al siguietne eslabón.
            _next = next;
        }

        // Clase 11: Método que implementa la funcionalidad del middleware
        public async Task InvokeAsync(HttpContext context)
        {
            // Clase 11: Antes de procesar el controlador
            Console.WriteLine(">>> Entrando al middleware de errores");

            try
            {
                // Clase 11: Invocación al controlador o siguiente middleware
                await _next(context);
            } catch(Exception ex)
            {
                // Clase 11: Manejo de errores originados en los controladores.
                HttpResponse response = context.Response;
                switch (ex)
                {
                    case ArgumentNullException:
                    case ArgumentException:
                        response.StatusCode = 400;
                        break;

                    case LoginException:
                        response.StatusCode = 403;
                        break;

                    default:
                        response.StatusCode = 500;
                        break;
                }

                await response.WriteAsJsonAsync(new { result = false, error = ex.Message });
            }

            // Clase 11: Luego de procesar el controlador
            Console.WriteLine("<<< Saliendo del middleware de errores");
        }
    }
}
