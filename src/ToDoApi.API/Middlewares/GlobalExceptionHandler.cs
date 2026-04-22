using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ToDoApi.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;


        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exceção não tratada: {Message}", exception.Message);

            var problem = exception switch
            {
                KeyNotFoundException e => new ProblemDetails
                {
                    Title = "Recurso não encontrado.",
                    Detail = e.Message,
                    Status = StatusCodes.Status404NotFound
                },
                ArgumentException e => new ProblemDetails
                {
                    Title = "Requisição inválida.",
                    Detail = e.Message,
                    Status = StatusCodes.Status400BadRequest
                },
                InvalidOperationException e => new ProblemDetails
                {
                    Title = "Operação inválida.",
                    Detail = e.Message,
                    Status = StatusCodes.Status409Conflict
                },
                _ => new ProblemDetails
                {
                    Title = "Erro interno do servidor.",
                    Detail = "Contate o suporte.",
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            httpContext.Response.StatusCode = problem.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
    }
}
    
