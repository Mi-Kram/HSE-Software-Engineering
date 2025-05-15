using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Interfaces;
using FileStoringService.Domain.Models;
using FileStoringService.Infrastructure.Persistence;
using FileStoringService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace FileStoringService.Infrastructure
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

            string? storageAddress = configuration.GetSection(ApplicationVariables.SRORAGE_ADDRESS).Value;
            if (string.IsNullOrWhiteSpace(storageAddress)) throw new EnvVariableException(ApplicationVariables.SRORAGE_ADDRESS);

            string? storageUser = configuration.GetSection(ApplicationVariables.SRORAGE_LOGIN).Value;
            if (string.IsNullOrWhiteSpace(storageUser)) throw new EnvVariableException(ApplicationVariables.SRORAGE_LOGIN);

            string? storagePassword = configuration.GetSection(ApplicationVariables.SRORAGE_PASSWORD).Value;
            if (string.IsNullOrWhiteSpace(storagePassword)) throw new EnvVariableException(ApplicationVariables.SRORAGE_PASSWORD);

            string? storageSsl = configuration.GetSection(ApplicationVariables.SRORAGE_SSL).Value;
            if (string.IsNullOrWhiteSpace(storageSsl)) throw new EnvVariableException(ApplicationVariables.SRORAGE_SSL);
            if (!bool.TryParse(storageSsl, out bool ssl)) throw new Exception($"{ApplicationVariables.SRORAGE_SSL}: неверное значение");

            // Добавление базы данных.
            collection.AddDbContext<WorkDbContext>(opts => opts.UseNpgsql(dbConnection));
            collection.AddScoped<IWorkRepository, WorkRepository>();

            // Добавление удалённого хранилища файлов.
            collection.AddMinio(x => x
                .WithEndpoint(storageAddress)
                .WithCredentials(storageUser, storagePassword)
                .WithSSL(ssl).Build());

            collection.AddScoped<IWorkStorageService, WorkStorageService>();
        }
    }
}
