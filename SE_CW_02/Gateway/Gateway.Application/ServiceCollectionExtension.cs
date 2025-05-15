using Gateway.Application.UseCases;
using Gateway.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Application
{
    /// <summary>
    /// Расширение интефейса <see cref="IServiceCollection"/> для подключения сервисов.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Подключение сервисов слоя Application.
        /// </summary>
        /// <param name="collection">Коллекция сервисов.</param>
        public static void AddApplication(this IServiceCollection collection)
        {
            // Добавление сервисов.
            collection.AddScoped<IStorageService, StorageService>();
            collection.AddScoped<IAnalyseService, AnalyseService>();
            collection.AddScoped<IWorkDeletionService, WorkDeletionService>();
        }
    }
}
