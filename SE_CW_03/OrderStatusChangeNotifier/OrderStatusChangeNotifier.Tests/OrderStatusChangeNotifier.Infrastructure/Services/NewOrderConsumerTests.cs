using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Moq;
using OrderStatusChangeNotifier.Infrastructure.Services;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Infrastructure.Services
{
    public class NewOrderConsumerExample(
        ILogger<NewOrderConsumer> logger, 
        IConsumer<string, string> consumer) : NewOrderConsumer(logger, consumer)
    {
        public async Task TestHandleAsync(string message, CancellationToken stoppingToken)
        {
            await base.HandleAsync(message, stoppingToken);
        }
    }

    public class NewOrderConsumerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldTriggerEvent_WithCorrectOrderId()
        {
            // Arrange
            var logger = new Mock<ILogger<NewOrderConsumer>>();
            var consumerMock = new Mock<IConsumer<string, string>>();
            var newOrderConsumer = new NewOrderConsumerExample(logger.Object, consumerMock.Object);

            string? triggeredOrderId = null;
            string? triggeredStatus = null;

            newOrderConsumer.OnOrdersStatusChangedEvent += (id, status) =>
            {
                triggeredOrderId = id;
                triggeredStatus = status;
            };

            string json = "{\"order_id\": \"abc123\"}";

            // Act
            await newOrderConsumer.TestHandleAsync(json, CancellationToken.None);

            // Assert
            Assert.Equal("abc123", triggeredOrderId);
            Assert.Equal("awaitForPayment", triggeredStatus);
        }
    }
}
