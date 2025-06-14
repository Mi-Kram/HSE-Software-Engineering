using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Moq;
using OrderStatusChangeNotifier.Infrastructure.Services;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Infrastructure.Services
{
    public class TestConsumer : MessageConsumer<TestConsumer>
    {
        public string? LastHandledMessage { get; private set; }

        public TestConsumer(ILogger<TestConsumer> logger, IConsumer<string, string> consumer)
            : base(logger, consumer) { }

        protected override Task HandleAsync(string message, CancellationToken stoppingToken)
        {
            LastHandledMessage = message;
            return Task.CompletedTask;
        }
    }

    public class MessageConsumerTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenLoggerIsNull()
        {
            var consumerMock = new Mock<IConsumer<string, string>>();
            Assert.Throws<ArgumentNullException>(() => new TestConsumer(null!, consumerMock.Object));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenConsumerIsNull()
        {
            var logger = new Mock<ILogger<TestConsumer>>();
            Assert.Throws<ArgumentNullException>(() => new TestConsumer(logger.Object, null!));
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            var logger = new Mock<ILogger<TestConsumer>>();
            var consumerMock = new Mock<IConsumer<string, string>>();

            var consumer = new TestConsumer(logger.Object, consumerMock.Object);

            consumer.Dispose();

            consumerMock.Verify(c => c.Close(), Times.Once);
            consumerMock.Verify(c => c.Dispose(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleValidMessage()
        {
            // Arrange
            var logger = new Mock<ILogger<TestConsumer>>();
            var consumerMock = new Mock<IConsumer<string, string>>();

            var message = new Message<string, string>
            {
                Value = "{\"test\": \"data\"}",
                Timestamp = new Timestamp(DateTime.UtcNow, TimestampType.CreateTime)
            };

            consumerMock.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                        .Returns(new ConsumeResult<string, string> { Message = message });

            var consumer = new TestConsumer(logger.Object, consumerMock.Object);
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(200); // Exit loop quickly

            // Act
            await consumer.StartAsync(tokenSource.Token);
            await consumer.StopAsync(tokenSource.Token);

            // Assert
            Assert.Equal("{\"test\": \"data\"}", consumer.LastHandledMessage);
        }
    }
}
