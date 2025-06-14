using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using OrderStatusChangeNotifier.Application.Interfaces;
using OrderStatusChangeNotifier.Application.Models;
using System.Text.Json;

namespace OrderStatusChangeNotifier.Infrastructure.Services
{
    public class PaidOrderConsumer(
    ILogger<PaidOrderConsumer> logger,
    IConsumer<string, string> consumer)
    : MessageConsumer<PaidOrderConsumer>(logger, consumer),
    IPaidOrderConsumer
    {
        public event OnOrdersStatusChangedDelegete? OnOrdersStatusChangedEvent;

        protected override Task HandleAsync(string message, CancellationToken stoppingToken)
        {
            using JsonDocument document = JsonDocument.Parse(message);

            string orderID = document.RootElement.GetProperty("order_id").GetString() ?? "";
            string status = document.RootElement.GetProperty("status").GetString() ?? "";

            OnOrdersStatusChangedEvent?.Invoke(orderID, status);
            return Task.CompletedTask;
        }
    }
}
