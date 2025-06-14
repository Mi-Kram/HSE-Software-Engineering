using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.API.Initializing;
using OrdersService.Domain.Exceptions;
using OrdersService.Domain.Models;

namespace OrdersService.Tests.OrdersService.API
{
    public class WebAppTests
    {
        [Fact]
        public void CheckEnvironmentVariables_ShouldThrow_WhenVariableMissing()
        {
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.DB_CONNECTION] = null
                })
                .Build();

            EnvVariableException ex = Assert.Throws<EnvVariableException>(() =>
                WebApp.CheckEnvironmentVariables(inMemoryConfig));

            Assert.Equal(ApplicationVariables.DB_CONNECTION, ex.EnvName);
        }

        [Fact]
        public void CheckEnvironmentVariables_ShouldPass_WhenAllVariablesExist()
        {
            Dictionary<string, string?> configValues = new()
            {
                [ApplicationVariables.DB_CONNECTION] = "conn",
                [ApplicationVariables.KAFKA] = "kafka",
                [ApplicationVariables.TOPIC_NEW_ORDER] = "topic_new_order",
                [ApplicationVariables.TOPIC_PAID_ORDER] = "topic_paid_order"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            WebApp.CheckEnvironmentVariables(config);
        }

        [Fact]
        public void AddServices_AddsExpectedServices()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                [ApplicationVariables.DB_CONNECTION] = "conn",
                [ApplicationVariables.KAFKA] = "kafka",
                [ApplicationVariables.TOPIC_NEW_ORDER] = "topic_new_order",
                [ApplicationVariables.TOPIC_PAID_ORDER] = "topic_paid_order"
            });

            WebApp.AddServices(builder);

            // Проверяем, что зарегистрирован хотя бы один контроллер
            var controllerService = builder.Services.BuildServiceProvider().GetService(typeof(IControllerFactory));
            Assert.NotNull(controllerService);
        }
    }
}
