using FileStoringService.Application;
using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FileStoringService.Tests.Application
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddApplication_Tests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(_ => new Mock<IWorkRepository>().Object);
            services.AddSingleton(_ => new Mock<IWorkStorageService>().Object);

            services.AddApplication();

            Assert.Contains(services, x => x.ServiceType == typeof(IStreamHasherService));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkService));
        }
    }
}
