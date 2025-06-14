using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Application.UseCases
{
    public class CompleteOrderConnector(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CompleteOrderConnector> logger,
        TimeSpan reserveTime) : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        private readonly ILogger<CompleteOrderConnector> logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            IOrdersService ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

            PaidOrderMessage? msg = await ordersService.TryReservePaidOrderMessageAsync(reserveTime);
            if (msg == null) return false;

            await ordersService.CompleteOrderAsync(msg);
            return true;
        }
    }
}
