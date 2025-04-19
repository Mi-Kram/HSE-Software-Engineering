using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.UseCases;
using Moq;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Statistics;

namespace SE_HW_02.Tests.UseCases
{
    public class ServicecollectionExtensionsTests
    {
        [Fact]
        public void AddUseCasesTests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            Mock<IFeedingScheduleRepository> scheduleRepository = new();
            scheduleRepository.Setup(x => x.GetAll()).Returns(() => []);
            services.AddSingleton(scheduleRepository.Object);

            services.AddUseCases();

            ServiceProvider provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetRequiredService<IAnimalService>());
            Assert.NotNull(provider.GetRequiredService<IEnclosureService>());
            Assert.NotNull(provider.GetRequiredService<IAnimalTransferService>());
            Assert.NotNull(provider.GetRequiredService<IFeedingOrganizationService>());
            Assert.NotNull(provider.GetRequiredService<IZooStatisticsService>());
            Assert.NotNull(provider.GetRequiredService<IFeedingMasterService>());
        }
    }
}
