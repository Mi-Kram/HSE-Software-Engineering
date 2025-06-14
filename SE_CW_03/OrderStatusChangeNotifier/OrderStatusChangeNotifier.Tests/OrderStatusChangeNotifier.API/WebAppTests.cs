using Microsoft.Extensions.Configuration;
using OrderStatusChangeNotifier.API.Initializing;
using OrderStatusChangeNotifier.Domain.Exceptions;
using OrderStatusChangeNotifier.Domain.Models;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.API
{
    public class WebAppTests
    {
        [Fact]
        public void CheckEnvironmentVariables_ShouldThrow_WhenVariableMissing()
        {
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.KAFKA] = null
                })
                .Build();

            EnvVariableException ex = Assert.Throws<EnvVariableException>(() =>
                WebApp.CheckEnvironmentVariables(inMemoryConfig));

            Assert.Equal(ApplicationVariables.KAFKA, ex.EnvName);
        }

        [Fact]
        public void CheckEnvironmentVariables_ShouldPass_WhenAllVariablesExist()
        {
            Dictionary<string, string?> configValues = new()
            {
                [ApplicationVariables.KAFKA] = "kafka",
                [ApplicationVariables.TOPIC_NEW_ORDER] = "topic_new_order",
                [ApplicationVariables.TOPIC_PAID_ORDER] = "topic_paid_order"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            WebApp.CheckEnvironmentVariables(config);
        }
    }
}
