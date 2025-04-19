using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Infrastructure.Repositories;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;
using SE_HW_02.UseCases.Statistics;
using SE_HW_02.Web.Controllers;

namespace SE_HW_02.Tests.Web.Controllers
{
    public class StatisticsControllerTests
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();
            services.AddSingleton<IZooStatisticsService, ZooStatisticsService>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test()
        {
            IServiceProvider provider = GetServiceProvider();
            IZooStatisticsService service = provider.GetRequiredService<IZooStatisticsService>();
            StatisticsController controller = new(service);

            Assert.NotNull(controller.GetAnimalsStatistics());
            Assert.NotNull(controller.GetEnclosuresStatistics());
            Assert.NotNull(controller.GetFeedingStatistics());
        }
    }
}
