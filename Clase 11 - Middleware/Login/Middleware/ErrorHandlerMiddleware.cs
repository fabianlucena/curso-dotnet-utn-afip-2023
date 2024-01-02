using Logic.Session;

namespace Login.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(">>> Entrando al middleware de errores");

            try
            {
                await _next(context);
            } catch(Exception ex)
            {
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

            Console.WriteLine("<<< Saliendo del middleware de errores");
        }
    }
}
