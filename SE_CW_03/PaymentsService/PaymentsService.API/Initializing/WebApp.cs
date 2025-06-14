using PaymentsService.Application;
using PaymentsService.Domain.Exceptions;
using PaymentsService.Domain.Models;
using PaymentsService.Infrastructure;
using PaymentsService.Infrastructure.Persistence;
using PaymentsService.API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace PaymentsService.API.Initializing
{
    public static class WebApp
    {
        public static async Task<WebApplication> CreateAsync(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            CheckEnvironmentVariables(builder.Configuration);
            AddServices(builder);

            WebApplication app = builder.Build();

            await InitializeDbAsync(app);
            ConfigureRequestPipeline(app);

            return app;
        }

        public static void CheckEnvironmentVariables(IConfiguration configuration)
        {
            string[] names = [
                ApplicationVariables.DB_CONNECTION, ApplicationVariables.KAFKA,
                ApplicationVariables.TOPIC_NEW_ORDER, ApplicationVariables.TOPIC_PAID_ORDER];

            foreach (string name in names)
            {
                if (configuration.GetValue<string>(name) == null) throw new EnvVariableException(name);
            }
        }

        public static void ConfigureRequestPipeline(WebApplication app)
        {
            app.MapGet("/api/health", async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Healthy");
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var logger = app.Services.GetRequiredService<ILogger<ExceptionInterceptorMiddleware>>();
            app.Use(new ExceptionInterceptorMiddleware(logger).Handle);

            app.MapControllers();
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddUseCases();
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static async Task InitializeDbAsync(WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
