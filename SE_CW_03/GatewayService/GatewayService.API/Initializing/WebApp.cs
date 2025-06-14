using GatewayService.Domain.Exceptions;
using GatewayService.Domain.Models;
using GatewayService.API.Middleware;
using Yarp.ReverseProxy.Configuration;
using GatewayService.API.Initializing.Swagger;

namespace GatewayService.API.Initializing
{
    public static class WebApp
    {
        public static Task<WebApplication> CreateAsync(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            CheckEnvironmentVariables(builder.Configuration);
            AddServices(builder);

            WebApplication app = builder.Build();

            app.UseStaticFiles();
            ConfigureRequestPipeline(app);

            return Task.FromResult(app);
        }

        public static void CheckEnvironmentVariables(IConfiguration configuration)
        {
            string[] names = [ApplicationVariables.ORDER_STATUS_NOTIFIER_SERVER,
                ApplicationVariables.ORDERS_SERVER, ApplicationVariables.PAYMENTS_SERVER];

            foreach (string name in names)
            {
                if (configuration.GetValue<string>(name) == null) throw new EnvVariableException(name);
            }
        }

        public static void AddReverseProxy(WebApplicationBuilder builder)
        {
            string notifierAddress = Environment.GetEnvironmentVariable(ApplicationVariables.ORDER_STATUS_NOTIFIER_SERVER) ?? "";
            string ordersAddress = Environment.GetEnvironmentVariable(ApplicationVariables.ORDERS_SERVER) ?? "";
            string paymentsAddress = Environment.GetEnvironmentVariable(ApplicationVariables.PAYMENTS_SERVER) ?? "";

            builder.Services.AddReverseProxy()
            .LoadFromMemory(
                [
                    new RouteConfig
                    {
                        RouteId = "websocket-route",
                        ClusterId = "order-status-notifier-cluster",
                        Match = new RouteMatch
                        {
                            Path = "/ws/{**catch-all}"
                        }
                    },
                    new RouteConfig
                    {
                        RouteId = "orders-route",
                        ClusterId = "orders-cluster",
                        Match = new RouteMatch
                        {
                            Path = "/api/orders/{**catch-all}"
                        }
                    },
                    new RouteConfig
                    {
                        RouteId = "payments-route",
                        ClusterId = "payments-cluster",
                        Match = new RouteMatch
                        {
                            Path = "/api/accounts/{**catch-all}"
                        }
                    }
                ],
                [
                    new ClusterConfig
                    {
                        ClusterId = "order-status-notifier-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            ["push-service"] = new DestinationConfig
                            {
                                Address = notifierAddress
                            }
                        }
                    },
                    new ClusterConfig
                    {
                        ClusterId = "orders-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            ["orders-service"] = new DestinationConfig
                            {
                                Address = ordersAddress
                            }
                        }
                    },
                    new ClusterConfig
                    {
                        ClusterId = "payments-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            ["accounts-service"] = new DestinationConfig
                            {
                                Address = paymentsAddress
                            }
                        }
                    }
                ]
            );
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

            app.UseSwagger();
            app.UseSwaggerUI();

            var logger = app.Services.GetRequiredService<ILogger<ExceptionInterceptorMiddleware>>();
            app.Use(new ExceptionInterceptorMiddleware(logger).Handle);

            app.MapReverseProxy();
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.OperationFilter<CreateAccountFormData>();
                opt.OperationFilter<CreateOrderFormData>();
                opt.OperationFilter<TopUpBalanceFormData>();
            });

            AddReverseProxy(builder);
        }
    }
}
