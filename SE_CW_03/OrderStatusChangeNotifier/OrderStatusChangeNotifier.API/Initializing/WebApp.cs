using OrderStatusChangeNotifier.Application;
using OrderStatusChangeNotifier.Domain.Exceptions;
using OrderStatusChangeNotifier.Domain.Models;
using OrderStatusChangeNotifier.Infrastructure;
using OrderStatusChangeNotifier.API.Middleware;
using System.Net.WebSockets;
using OrderStatusChangeNotifier.Domain.Interfaces;

namespace OrderStatusChangeNotifier.API.Initializing
{
    public static class WebApp
    {
        public static Task<WebApplication> CreateAsync(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            CheckEnvironmentVariables(builder.Configuration);
            AddServices(builder);

            WebApplication app = builder.Build();

            ConfigureRequestPipeline(app);
            return Task.FromResult(app);
        }

        public static void CheckEnvironmentVariables(IConfiguration configuration)
        {
            string[] names = [ ApplicationVariables.KAFKA,
                ApplicationVariables.TOPIC_NEW_ORDER, ApplicationVariables.TOPIC_PAID_ORDER];

            foreach (string name in names)
            {
                if (configuration.GetValue<string>(name) == null) throw new EnvVariableException(name);
            }
        }

        public static void ConfigureRequestPipeline(WebApplication app)
        {
            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin();
                opt.AllowAnyMethod();
                opt.AllowAnyHeader();
            });

            app.MapGet("/api/health", async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Healthy");
            });

            AddWebSocket(app);

            var logger = app.Services.GetRequiredService<ILogger<ExceptionInterceptorMiddleware>>();
            app.Use(new ExceptionInterceptorMiddleware(logger).Handle);
        }

        public static void AddWebSocket(WebApplication app)
        {
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path != "/ws")
                {
                    await next();
                    return;
                }

                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                await Task.Yield();

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                IWebSocketManager wsManager = context.RequestServices.GetRequiredService<IWebSocketManager>();
                await wsManager.HandleWebSocketConnectionAsync(webSocket);
            });
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddUseCases();
            builder.Services.AddCors();
        }
    }
}
