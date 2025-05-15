using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Domain.Models;
using FileAnalysisService.Infrastructure.Persistence;
using FileAnalysisService.Infrastructure.Persistence.Context;
using FileAnalysisService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace FileAnalysisService.Infrastructure
{
    /// <summary>
    /// Расширение интефейса <see cref="IServiceCollection"/> для подключения сервисов.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Подключение сервисов слоя Infrastructure.
        /// </summary>
        /// <param name="collection">Коллекция сервисов.</param>
        public static void AddInfrastructure(this IServiceCollection collection, IConfiguration configuration)
        {
            // Чтение переменных среды.
            string? dbConnection = configuration.GetSection(ApplicationVariables.DB_CONNECTION).Value;
            if (string.IsNullOrWhiteSpace(dbConnection)) throw new EnvVariableException(ApplicationVariables.DB_CONNECTION);

            string? storageAddress = configuration.GetSection(ApplicationVariables.WORDS_CLOUD_SRORAGE_ADDRESS).Value;
            if (string.IsNullOrWhiteSpace(storageAddress)) throw new EnvVariableException(ApplicationVariables.WORDS_CLOUD_SRORAGE_ADDRESS);

            string? storageUser = configuration.GetSection(ApplicationVariables.WORDS_CLOUD_SRORAGE_LOGIN).Value;
            if (string.IsNullOrWhiteSpace(storageUser)) throw new EnvVariableException(ApplicationVariables.WORDS_CLOUD_SRORAGE_LOGIN);

            string? storagePassword = configuration.GetSection(ApplicationVariables.WORDS_CLOUD_SRORAGE_PASSWORD).Value;
            if (string.IsNullOrWhiteSpace(storagePassword)) throw new EnvVariableException(ApplicationVariables.WORDS_CLOUD_SRORAGE_PASSWORD);

            string? storageSsl = configuration.GetSection(ApplicationVariables.WORDS_CLOUD_SRORAGE_SSL).Value;
            if (string.IsNullOrWhiteSpace(storageSsl)) throw new EnvVariableException(ApplicationVariables.WORDS_CLOUD_SRORAGE_SSL);
            if (!bool.TryParse(storageSsl, out bool ssl)) throw new Exception($"{ApplicationVariables.WORDS_CLOUD_SRORAGE_SSL}: неверное значение");

            // Кофигурация подключения к базе данных.
            collection.AddDbContext<ApplicationDbContext>(opts => opts.UseNpgsql(dbConnection));

            // Добавление репозиториев.
            collection.AddScoped<IAnalyseReportRepository, AnalyseReportRepository>();
            collection.AddScoped<IComparisonReportRepository, ComparisonReportRepository>();
            collection.AddScoped<IWorkRepository, WorkRepository>();

            // Добавление сервисов.
            collection.AddScoped<IComparisonTool, CombinedComparisonTool>();
            collection.AddScoped<IWordsCloudService, WordsCloudService>();
            collection.AddScoped<IWorkStorageService, WorkStorageService>();

            // Добавление удалённого хранилища изображений облака слов.
            collection.AddMinio(x => x
                .WithEndpoint(storageAddress)
                .WithCredentials(storageUser, storagePassword)
                .WithSSL(ssl).Build());

            collection.AddScoped<IAnalyseStorageService, AnalyseStorageService>();
        }
    }
}
