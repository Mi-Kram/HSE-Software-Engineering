using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrdersService.Domain.Interfaces;
using OrdersService.Domain.Interfaces.MessagesRepository;
using OrdersService.Infrastructure;
using OrdersService.Infrastructure.Persistence;
using OrdersService.Infrastructure.Services;
using System;

namespace OrdersService.Tests.OrdersService.Infrastructure
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

            Assert.Contains(services, x => x.ServiceType == typeof(OrdersDbContext));
            Assert.Contains(services, x => x.ServiceType == typeof(IOrdersRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(INewOrderMessagesRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IPaidOrderMessagesRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IDbTransaction));

            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(NewOrderProducer));
            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(OnPaidOrderResultConsumer));
        }
    }
}
