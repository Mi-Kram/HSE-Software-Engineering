using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using OrderStatusChangeNotifier.Application.Interfaces;
using OrderStatusChangeNotifier.Application.Models;
using System.Text.Json;

namespace OrderStatusChangeNotifier.Infrastructure.Services
{
    public class NewOrderConsumer(
        ILogger<NewOrderConsumer> logger,
        IConsumer<string, string> consumer) 
        : MessageConsumer<NewOrderConsumer>(logger, consumer),
        INewOrderConsumer
    {
        public event OnOrdersStatusChangedDelegete? OnOrdersStatusChangedEvent;

        protected override Task HandleAsync(string message, CancellationToken stoppingToken)
        {
            using JsonDocument document = JsonDocument.Parse(message);

            string orderID = document.RootElement.GetProperty("order_id").GetString() ?? "";

            OnOrdersStatusChangedEvent?.Invoke(orderID, "awaitForPayment");
            return Task.CompletedTask;
        }
    }
}
