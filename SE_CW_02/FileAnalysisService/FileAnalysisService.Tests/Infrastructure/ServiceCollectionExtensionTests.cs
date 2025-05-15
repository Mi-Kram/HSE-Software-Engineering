using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Interfaces;
using FileAnalysisService.Infrastructure;
using FileAnalysisService.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Moq;

namespace FileAnalysisService.Tests.Infrastructure
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

            Assert.Contains(services, x => x.ServiceType == typeof(ApplicationDbContext));
            Assert.Contains(services, x => x.ServiceType == typeof(IAnalyseReportRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IComparisonReportRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkRepository));
            Assert.Contains(services, x => x.ServiceType == typeof(IComparisonTool));
            Assert.Contains(services, x => x.ServiceType == typeof(IWordsCloudService));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkStorageService));
            Assert.Contains(services, x => x.ServiceType == typeof(IMinioClient));
            Assert.Contains(services, x => x.ServiceType == typeof(IAnalyseStorageService));
        }
    }
}
