using FileAnalysisService.Application;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysisService.Tests.Application
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddApplication_Tests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddApplication();

            Assert.Contains(services, x => x.ServiceType == typeof(IAnalyseTool));
            Assert.Contains(services, x => x.ServiceType == typeof(IAnalyseService));
            Assert.Contains(services, x => x.ServiceType == typeof(IComparisonService));
            Assert.Contains(services, x => x.ServiceType == typeof(IWorkService));
        }
    }
}
