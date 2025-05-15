using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Primitives;

namespace FileAnalysisService.Api.Middleware
{
    /// <summary>
    /// Обработчик запросов: проверка доступа к сервису.
    /// </summary>
    public class AuthorizationMiddleware
    {
        /// <summary>
        /// Токены доступа.
        /// </summary>
        private readonly HashSet<string> tokens = [];

        public AuthorizationMiddleware(IEnumerable<string> tokens)
        {
            ArgumentNullException.ThrowIfNull(tokens, nameof(tokens));
            this.tokens = [.. tokens];
        }

        /// <summary>
        /// Обработчик запросов.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <param name="next">Делегат дальнейшей обработки запроса.</param>
        public async Task Handle(HttpContext context, Func<Task> next)
        {
            // Получение и проверка токена.
            if (!context.Request.Headers.TryGetValue("token", out StringValues tokenList) ||
                tokenList.FirstOrDefault() is not { } token ||
                !tokens.Contains(token))
            {
                // Отправка ошибки.

                JsonResult result = new(new
                {
                    error = "Unauthorized"
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                context.Response.Clear();
                ActionContext actionContext = new(context, context.GetRouteData(), new ActionDescriptor());
                await result.ExecuteResultAsync(actionContext);

                return;
            }

            await next();
        }
    }
}
