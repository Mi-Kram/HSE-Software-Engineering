using Gateway.Application.Interfaces;
using Gateway.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Tests.Infrastructure
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddInfrastructure_Tests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddInfrastructure();

            Assert.Contains(services, x => x.ServiceType == typeof(IHttpRedirectionService));
            Assert.Contains(services, x => x.ServiceType == typeof(IOuterWorkDeletionService));
            Assert.Contains(services, x => x.ServiceType == typeof(IOuterStorageService));
            Assert.Contains(services, x => x.ServiceType == typeof(IOuterAnalyseService));
        }
    }
}
