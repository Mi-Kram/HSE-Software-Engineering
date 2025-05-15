using Gateway.Api.Middleware;
using Gateway.Application;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;
using Gateway.Infrastructure;

namespace Gateway.Api.Initializing
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
        public static Task<WebApplication> CreateAsync(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Проверка наличия переменных среды.
            CheckEnvironmentVariables(builder.Configuration);

            // Добавление сервисов.
            AddServices(builder);

            WebApplication app = builder.Build();

            // Настройка обработки запросов.
            ConfigureRequestPipeline(app);

            return Task.FromResult(app);
        }

        /// <summary>
        /// Проверка наличия переменных среды.
        /// </summary>
        public static void CheckEnvironmentVariables(IConfiguration configuration)
        {
            string[] names = [ ApplicationVariables.STORAGE_SERVER, ApplicationVariables.STORAGE_TOKEN,
                ApplicationVariables.ANALYSE_SERVER, ApplicationVariables.ANALYSE_TOKEN ];

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
            app.UseSwagger();
            app.UseSwaggerUI();

            // Перехват исключений.
            var exceptionLogger = app.Services.GetRequiredService<ILogger<ExceptionInterceptorMiddleware>>();
            app.Use(new ExceptionInterceptorMiddleware(exceptionLogger).Handle);

            // Проверка авторизации.
            app.UseAuthorization();

            app.MapControllers();
        }

        /// <summary>
        /// Добавление сервисов.
        /// </summary>
        /// <param name="builder">Builder приложения.</param>
        public static void AddServices(WebApplicationBuilder builder)
        {
            // Добавление сервисов.
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();
            builder.Services.AddControllers();
            
            // Сервисы для Swagger генерации.
            builder.Services.AddEndpointsApiExplorer();

            // Добавление Swagger сервиса.
            builder.Services.AddSwaggerGen(opts =>
            {
                // Добавление формы для загрузки работы.
                opts.OperationFilter<SwaggerAddUploadWorkFormData>();
            });
        }
    }
}
