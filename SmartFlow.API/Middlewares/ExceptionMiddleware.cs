using System.Net;
using System.Text.Json;

namespace SmartFlow.API.Middlewares
{
    // Intercepte TOUTES les exceptions non gérées de l'application, à un seul endroit
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // laisse la requête continuer normalement
            }
            catch (Exception ex)
            {
                // On journalise l'erreur complète côté serveur (utile pour le débogage)
                _logger.LogError(ex, "Une erreur non gérée s'est produite.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Mais on renvoie au client un message générique, sans détails techniques sensibles
                var response = new { message = "Une erreur interne est survenue. Veuillez réessayer." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}