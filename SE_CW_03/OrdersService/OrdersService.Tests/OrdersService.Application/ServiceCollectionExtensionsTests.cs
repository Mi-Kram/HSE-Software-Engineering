using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Application;
using OrdersService.Application.UseCases;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Tests.OrdersService.Application
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddUseCases_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(_ => new Mock<IServiceScopeFactory>().Object);
            services.AddSingleton(_ => new Mock<ILogger<CompleteOrderConnector>>().Object);

            services.AddUseCases();
            Assert.Contains(services, x => x.ServiceType == typeof(IOrdersService));
            Assert.Contains(services, x => x.ImplementationFactory?.Method?.ReturnType == typeof(CompleteOrderConnector));
        }
    }
}
