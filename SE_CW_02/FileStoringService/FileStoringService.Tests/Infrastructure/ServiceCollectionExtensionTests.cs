using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FileStoringService.Infrastructure;
using Microsoft.Extensions.Configuration;
using FileStoringService.Infrastructure.Persistence;
using Minio;

namespace FileStoringService.Tests.Infrastructure
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddInfrastructure_Tests()
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

            Assert.Contains(services, x => x.ServiceType == typeof(WorkDbContext));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IMinioClient));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkStorageService));
        }
    }
}
