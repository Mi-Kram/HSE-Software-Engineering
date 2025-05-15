using Gateway.Api.Initializing;
using Gateway.Domain.Exceptions;
using Gateway.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Gateway.Tests.Api
{
    public class WebAppTests
    {
        [Fact]
        public void CheckEnvironmentVariables_ShouldThrow_WhenVariableMissing()
        {
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.STORAGE_SERVER] = null
                })
                .Build();

            EnvVariableException ex = Assert.Throws<EnvVariableException>(() =>
                WebApp.CheckEnvironmentVariables(inMemoryConfig));

            Assert.Equal(ApplicationVariables.STORAGE_SERVER, ex.EnvName);
        }

        [Fact]
        public void CheckEnvironmentVariables_ShouldPass_WhenAllVariablesExist()
        {
            Dictionary<string, string?> configValues = new()
            {
                [ApplicationVariables.STORAGE_SERVER] = "store",
                [ApplicationVariables.STORAGE_TOKEN] = "token1",
                [ApplicationVariables.ANALYSE_SERVER] = "analyse",
                [ApplicationVariables.ANALYSE_TOKEN] = "token2"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            WebApp.CheckEnvironmentVariables(config);
        }
    }
}
