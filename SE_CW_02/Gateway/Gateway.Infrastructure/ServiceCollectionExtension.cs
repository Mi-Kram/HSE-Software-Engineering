using Gateway.Application.Interfaces;
using Gateway.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Infrastructure
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
        public static void AddInfrastructure(this IServiceCollection collection)
        {
            collection.AddScoped<IHttpRedirectionService, HttpRedirectionService>();

            // Взаимодействие с внешними сервисами.
            collection.AddScoped<IOuterWorkDeletionService, WorkDeletionService>();
            collection.AddScoped<IOuterStorageService, StorageService>();
            collection.AddScoped<IOuterAnalyseService, AnalyseService>();
        }
    }
}
