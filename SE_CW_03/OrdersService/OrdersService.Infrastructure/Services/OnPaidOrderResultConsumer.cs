using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;
using System.Text.Json;

namespace OrdersService.Infrastructure.Services
{
    public sealed class OnPaidOrderResultConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<OnPaidOrderResultConsumer> logger,
        IConsumer<string, string> consumer) : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        private readonly ILogger<OnPaidOrderResultConsumer> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IConsumer<string, string> consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await HandleAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError("{Message}", ex.Message);
                }
            }
        }

        private async Task HandleAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IOrdersService ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

            ConsumeResult<string, string> result = consumer.Consume(stoppingToken);

            using JsonDocument document = JsonDocument.Parse(result.Message.Value);

            Guid orderID = document.RootElement.GetProperty("order_id").GetGuid();
            string statusStr = document.RootElement.GetProperty("status").GetString() ?? "";
            OrderStatus status = Enum.Parse<OrderStatus>(statusStr);

            await ordersService.OnPaymentResultReceivedAsync(orderID, status);
            consumer.Commit(result);
        }

        public override void Dispose()
        {
            consumer.Close();
            consumer.Dispose();
            base.Dispose();
        }
    }
}
