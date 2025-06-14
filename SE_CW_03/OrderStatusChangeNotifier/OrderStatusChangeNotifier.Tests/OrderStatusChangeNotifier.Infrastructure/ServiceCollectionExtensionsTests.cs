using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderStatusChangeNotifier.Infrastructure;
using OrderStatusChangeNotifier.Infrastructure.Services;
using System.Data;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Infrastructure
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddInfrastructure_Test()
        {
            IServiceCollection services = new ServiceCollection();

            Mock<IConfigurationSection> mockConfigurationSection = new();
            mockConfigurationSection
                .Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(mockConfigurationSection.Object);
            mockConfigurationSection
                .Setup(x => x.Value)
                .Returns("false");

            Mock<IConfiguration> mockConfiguration = new();
            mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockConfigurationSection.Object);

            services.AddInfrastructure(mockConfiguration.Object);

            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(NewOrderConsumer));
            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(PaidOrderConsumer));
        }
    }
}
