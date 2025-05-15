using FileStoringService.Api.Middleware;
using FileStoringService.Application;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Models;
using FileStoringService.Infrastructure;
using FileStoringService.Infrastructure.Persistence;

namespace FileStoringService.Api.Initializing
{
    /// <summary>
    /// Класс для создания веб приложения.
    /// </summary>
    public static class WebApp
    {
        /// <summary>
        /// Создание веб приложения.
        /// </summary>
        /// <param name="args">CLI параметры.</param>
        /// <returns>Веб приложение.</returns>
        public static async Task<WebApplication> CreateAsync(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Проверка наличия переменных среды.
            CheckEnvironmentVariables(builder.Configuration);

            // Добавление сервисов.
            AddServices(builder);

            WebApplication app = builder.Build();

            // Инициализация базы данных.
            await InitializeDbAsync(app);

            // Настройка обработки запросов.
            ConfigureRequestPipeline(app);

            return app;
        }

        /// <summary>
        /// Проверка наличия переменных среды.
        /// </summary>
        public static void CheckEnvironmentVariables(IConfiguration configuration)
        {
            string[] names = [ ApplicationVariables.TOKEN,
                ApplicationVariables.DB_CONNECTION, ApplicationVariables.SRORAGE_ADDRESS,
                ApplicationVariables.SRORAGE_LOGIN, ApplicationVariables.SRORAGE_PASSWORD,
                ApplicationVariables.SRORAGE_BUCKET, ApplicationVariables.SRORAGE_SSL];

            foreach (string name in names)
            {
                if (configuration.GetValue<string>(name) == null) throw new EnvVariableException(name);
            }
        }

        /// <summary>
        /// Настройка обработки запросов.
        /// </summary>
        /// <param name="app">Веб приложение.</param>
        public static void ConfigureRequestPipeline(WebApplication app)
        {
            // Https перенапрвление.
            // app.UseHttpsRedirection();

            // Добавление Cors Policy.
            app.UseCors(opt =>
            {
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();
                opt.AllowAnyOrigin();
            });

            // Добавление Swagger.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Перехват исключений.
            var logger = app.Services.GetRequiredService<ILogger<ExceptionInterceptorMiddleware>>();
            app.Use(new ExceptionInterceptorMiddleware(logger).Handle);

            // Проверка авторизации.
            app.UseAuthorization();
            UseAuthMiddleware(app);

            app.MapControllers();
        }

        /// <summary>
        /// Добавление сервисов.
        /// </summary>
        /// <param name="builder">Builder приложения.</param>
        public static void AddServices(WebApplicationBuilder builder)
        {
            // Добавление сервисов.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddControllers();
            
            // Сервисы для Swagger генерации.
            builder.Services.AddEndpointsApiExplorer();

            // Добавление Swagger сервиса.
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen(opt =>
                {
                    // Добавления в Headers поля token.
                    opt.AddOperationFilterInstance(new SwaggerAddTokenHeader());
                });
            }
        }

        /// <summary>
        /// Добавление middleware.
        /// </summary>
        /// <param name="app">Веб приложение.</param>
        public static void UseAuthMiddleware(WebApplication app)
        {
            // Токены доступа.
            string? tokensStr = app.Configuration.GetValue<string>(ApplicationVariables.TOKEN) ?? throw new EnvVariableException(ApplicationVariables.TOKEN);
            IEnumerable<string> tokens = tokensStr.Split(';', StringSplitOptions.RemoveEmptyEntries);

            // Проверка наличия токенов.
            if (!tokens.Any()) throw new Exception("Нет токенов авторизации");

            // Подключение Middleware.
            app.Use(new AuthorizationMiddleware(tokens).Handle);
        }

        /// <summary>
        /// Инициализация базы данных.
        /// </summary>
        /// <param name="app">Веб приложение.</param>
        public static async Task InitializeDbAsync(WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            WorkDbContext context = scope.ServiceProvider.GetRequiredService<WorkDbContext>();
            await context.Database.EnsureCreatedAsync();
        }
    }
}
