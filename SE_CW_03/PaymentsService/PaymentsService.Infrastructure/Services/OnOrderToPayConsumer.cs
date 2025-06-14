using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;
using System.Text.Json;

namespace PaymentsService.Infrastructure.Services
{
    public class OnOrderToPayConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<OnOrderToPayConsumer> logger,
        IConsumer<string, string> consumer) : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        private readonly ILogger<OnOrderToPayConsumer> logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

            ConsumeResult<string, string> result = consumer.Consume(stoppingToken);

            using JsonDocument document = JsonDocument.Parse(result.Message.Value);
            
            OrderToPayMessage message = new()
            {
                OrderID = document.RootElement.GetProperty("order_id").GetGuid(),
                UserID = document.RootElement.GetProperty("user_id").GetInt32(),
                CreatedAt = document.RootElement.GetProperty("created_at").GetDateTime(),
                Bill = document.RootElement.GetProperty("bill").GetDecimal(),
                Reserved = DateTime.Now
            };

            await accountService.OnNewOrderToPayAsync(message);
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
