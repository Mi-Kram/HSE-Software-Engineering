using Gateway.Application;
using Gateway.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Tests.Application
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddApplication_Tests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddApplication();

            Assert.Contains(services, x => x.ServiceType == typeof(IStorageService));
            Assert.Contains(services, x => x.ServiceType == typeof(IAnalyseService));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkDeletionService));
        }
    }
}
