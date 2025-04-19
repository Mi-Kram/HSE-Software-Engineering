using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.UseCases.Statistics;

namespace SE_HW_02.UseCases
{
    /// <summary>
    /// Добавление метода <see cref="AddUseCases(IServiceCollection)"/>.
    /// </summary>
    public static class ServicecollectionExtensions
    {
        /// <summary>
        /// Добаление сервисов слоя Application/UseCases.
        /// </summary>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            // Добавление сервисов логики.
            services.AddSingleton<IAnimalService, AnimalService>();
            services.AddSingleton<IEnclosureService, EnclosureService>();
            services.AddSingleton<IAnimalTransferService, AnimalTransferService>();
            services.AddSingleton<IFeedingOrganizationService>(FeedingOrganizationService.Initialize);
            services.AddSingleton<IZooStatisticsService, ZooStatisticsService>();
            services.AddSingleton<IFeedingMasterService, FeedingMasterService>();

            return services;
        }
    }
}
