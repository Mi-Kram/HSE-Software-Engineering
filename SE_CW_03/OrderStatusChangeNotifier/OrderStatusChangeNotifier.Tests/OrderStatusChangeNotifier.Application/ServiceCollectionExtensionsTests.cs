using Microsoft.Extensions.DependencyInjection;
using OrderStatusChangeNotifier.Application;
using OrderStatusChangeNotifier.Domain.Interfaces;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Application
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddUseCases_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddUseCases();

            Assert.Contains(services, x => x.ServiceType == typeof(IWebSocketManager));
        }
    }
}
