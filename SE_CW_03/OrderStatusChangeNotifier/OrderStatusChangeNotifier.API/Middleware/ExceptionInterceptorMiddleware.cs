using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace OrderStatusChangeNotifier.API.Middleware
{
    public class ExceptionInterceptorMiddleware(ILogger<ExceptionInterceptorMiddleware> logger)
    {
        private readonly ILogger<ExceptionInterceptorMiddleware> logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
