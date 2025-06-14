using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Moq;
using OrderStatusChangeNotifier.Infrastructure.Services;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Infrastructure.Services
{
    public class PaidOrderConsumerExample(
    ILogger<PaidOrderConsumer> logger,
    IConsumer<string, string> consumer) : PaidOrderConsumer(logger, consumer)
    {
        public async Task TestHandleAsync(string message, CancellationToken stoppingToken)
        {
            await base.HandleAsync(message, stoppingToken);
        }
    }

    public class PaidOrderConsumerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldTriggerEvent_WithCorrectOrderIdAndStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<PaidOrderConsumer>>();
            var consumerMock = new Mock<IConsumer<string, string>>();
            var paidOrderConsumer = new PaidOrderConsumerExample(logger.Object, consumerMock.Object);

            string? triggeredOrderId = null;
            string? triggeredStatus = null;

            paidOrderConsumer.OnOrdersStatusChangedEvent += (id, status) =>
            {
                triggeredOrderId = id;
                triggeredStatus = status;
            };

            string json = "{\"order_id\": \"xyz789\", \"status\": \"PAID\"}";

            // Act
            await paidOrderConsumer.TestHandleAsync(json, CancellationToken.None);

            // Assert
            Assert.Equal("xyz789", triggeredOrderId);
            Assert.Equal("PAID", triggeredStatus);
        }
    }
}
