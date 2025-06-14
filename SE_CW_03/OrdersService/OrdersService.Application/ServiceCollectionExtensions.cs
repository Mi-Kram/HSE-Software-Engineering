using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrdersService.Application.UseCases;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IOrdersService, UseCases.OrdersService>();
            AddCompleteOrderConnector(services);
        }

        private static void AddCompleteOrderConnector(IServiceCollection services)
        {
            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<CompleteOrderConnector> logger = x.GetRequiredService<ILogger<CompleteOrderConnector>>();
                return new CompleteOrderConnector(scopeFactory, logger, TimeSpan.FromSeconds(10));
            });
        }
    }
}
