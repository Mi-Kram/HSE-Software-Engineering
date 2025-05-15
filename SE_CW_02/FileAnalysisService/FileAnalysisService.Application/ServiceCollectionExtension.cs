using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysisService.Application
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
            // Добавление инструментов.
            collection.AddScoped<IAnalyseTool, DefaultAnalyseTool>();

            // Добавление сервисов.
            collection.AddScoped<IAnalyseService, AnalyseService>();
            collection.AddScoped<IComparisonService, ComparisonService>();
            collection.AddScoped<IWorkService, WorkService>();
        }
    }
}
