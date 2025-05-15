using FileStoringService.Application.Interfaces;
using FileStoringService.Application.UseCases;
using FileStoringService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.Application
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
            collection.AddSingleton<IStreamHasherService, SHA512StreamHasherService>();
            collection.AddScoped<IWorkService, WorkService>();
        }
    }
}
