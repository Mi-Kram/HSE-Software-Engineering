using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Domain.Interfaces.MessagesRepository;
using PaymentsService.Infrastructure;
using PaymentsService.Infrastructure.Persistence;
using PaymentsService.Infrastructure.Services;

namespace PaymentsService.Tests.PaymentsService.Infrastructure
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

            Assert.Contains(services, x => x.ServiceType == typeof(AppDbContext));
            Assert.Contains(services, x => x.ServiceType == typeof(IAccountRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IOrderToPayMessagesRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IPaidOrderMessagesRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IDbTransaction));

            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(OnOrderToPayConsumer));
            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(PaidOrderProducer));
        }
    }
}
