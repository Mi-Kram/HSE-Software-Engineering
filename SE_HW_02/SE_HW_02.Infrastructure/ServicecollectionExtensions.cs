using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Infrastructure
{
    /// <summary>
    /// Добавление метода <see cref="AddInfrastructure(IServiceCollection)"/>.
    /// </summary>
    public static class ServicecollectionExtensions
    {
        /// <summary>
        /// Добаление сервисов слоя Infrastructure.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Подключение репозиториев.
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

            return services;
        }
    }
}
