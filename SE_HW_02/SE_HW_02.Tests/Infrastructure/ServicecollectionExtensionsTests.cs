using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Infrastructure;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.Tests.Infrastructure
{
    public class ServicecollectionExtensionsTests
    {
        [Fact]
        public void AddInfrastructureTests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddInfrastructure();

            ServiceProvider provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetRequiredService<IAnimalRepository>());
            Assert.NotNull(provider.GetRequiredService<IEnclosureRepository>());
            Assert.NotNull(provider.GetRequiredService<IFeedingScheduleRepository>());
        }
    }
}
