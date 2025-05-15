using FileStoringService.Api.Initializing;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace FileStoringService.Tests.Api
{
    public class WebAppTests
    {
        [Fact]
        public void CheckEnvironmentVariables_ShouldThrow_WhenVariableMissing()
        {
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [ApplicationVariables.TOKEN] = null
                })
                .Build();

            EnvVariableException ex = Assert.Throws<EnvVariableException>(() =>
                WebApp.CheckEnvironmentVariables(inMemoryConfig));

            Assert.Equal(ApplicationVariables.TOKEN, ex.EnvName);
        }

        [Fact]
        public void CheckEnvironmentVariables_ShouldPass_WhenAllVariablesExist()
        {
            Dictionary<string, string?> configValues = new()
            {
                [ApplicationVariables.TOKEN] = "abc",
                [ApplicationVariables.DB_CONNECTION] = "conn",
                [ApplicationVariables.SRORAGE_ADDRESS] = "addr",
                [ApplicationVariables.SRORAGE_LOGIN] = "login",
                [ApplicationVariables.SRORAGE_PASSWORD] = "pwd",
                [ApplicationVariables.SRORAGE_BUCKET] = "bucket",
                [ApplicationVariables.SRORAGE_SSL] = "true"
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            WebApp.CheckEnvironmentVariables(config);
        }

        [Fact]
        public void UseAuthMiddleware_ShouldThrow_WhenTokensMissing()
        {
            var app = WebApplication.Create();

            var ex = Assert.Throws<EnvVariableException>(() => WebApp.UseAuthMiddleware(app));
            Assert.Equal(ApplicationVariables.TOKEN, ex.EnvName);
        }

        [Fact]
        public void UseAuthMiddleware_ShouldThrow_WhenTokenListEmpty()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration[ApplicationVariables.TOKEN] = "";

            var ex = Assert.Throws<Exception>(() => WebApp.UseAuthMiddleware(builder.Build()));
            Assert.Contains("Нет токенов авторизации", ex.Message);
        }

        [Fact]
        public void UseAuthMiddleware_ShouldAddMiddleware_WhenTokensPresent()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration[ApplicationVariables.TOKEN] = "token1;token2";

            WebApp.UseAuthMiddleware(builder.Build());
        }
    }
}
