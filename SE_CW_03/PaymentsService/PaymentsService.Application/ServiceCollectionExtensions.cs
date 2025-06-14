using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentsService.Application.UseCases;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            AddPaymentsService(services);
        }

        private static void AddPaymentsService(IServiceCollection services)
        {
            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<UseCases.PaymentsService> logger = x.GetRequiredService<ILogger<UseCases.PaymentsService>>();
                return new UseCases.PaymentsService(scopeFactory, logger, TimeSpan.FromSeconds(10));
            });
        }
    }
}
