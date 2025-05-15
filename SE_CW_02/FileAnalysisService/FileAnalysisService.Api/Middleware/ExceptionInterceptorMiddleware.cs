using FileAnalysisService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace FileAnalysisService.Api.Middleware
{
    /// <summary>
    /// Обработчик запросов: перехват исключений.
    /// </summary>
    public class ExceptionInterceptorMiddleware(ILogger<ExceptionInterceptorMiddleware> logger)
    {
        private readonly ILogger<ExceptionInterceptorMiddleware> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Обработчик запросов.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <param name="next">Делегат дальнейшей обработки запроса.</param>
        public async Task Handle(HttpContext context, Func<Task> next)
        {
            JsonResult result = new(new
            {
                error = "Ошибка сервера"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            try
            {
                await next();
                return;
            }
            // Не установлена переменная среды.
            catch (EnvVariableException ex)
            {
                logger.LogCritical("{EnvName}: переменная среды не найдена", ex.EnvName);
            }
            // Ошибка запроса пользователя.
            catch (ArgumentException ex)
            {
                result.Value = new { error = ex.Message };
                result.StatusCode = StatusCodes.Status400BadRequest;
            }
            // Другая ошибка сервера.
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
            }

            context.Response.Clear();
            ActionContext actionContext = new(context, context.GetRouteData(), new ActionDescriptor());
            await result.ExecuteResultAsync(actionContext);
        }
    }
}
