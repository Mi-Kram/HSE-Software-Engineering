using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Application.UseCases
{
    public class PaymentsService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<PaymentsService> logger,
        TimeSpan reserveTime) : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        private readonly ILogger<PaymentsService> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly TimeSpan reserveTime = TimeSpan.Zero < reserveTime ? reserveTime : throw new ArgumentException($"{nameof(reserveTime)} должен быть больше 0");

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (await HandleAsync()) continue;
                }
                catch (Exception ex)
                {
                    logger.LogError("{Message}", ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private async Task<bool> HandleAsync()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

            OrderToPayMessage? msg = await accountService.TryReserveOrderToPayMessageAsync(reserveTime);
            if (msg == null) return false;

            await accountService.PayForOrderAsync(msg);
            return true;
        }
    }
}
