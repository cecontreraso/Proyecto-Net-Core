using Microsoft.AspNetCore.Builder;

namespace WebApiAutores.Middlewares
{

    public static class LoguearRespuestaHTTPMiddlewaresExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddlewares>();
        }
    }

    public class LoguearRespuestaHTTPMiddlewares
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaHTTPMiddlewares> logger;

        public LoguearRespuestaHTTPMiddlewares(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddlewares> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;
                await siguiente(contexto); //le permito a la tuberia continuar


                //de aca en adelante se vana ejecutar cuando ya los middleawear posteriorres me devuelvan una respuesta
                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                logger.LogInformation(respuesta);
            }
        }
    }
}
