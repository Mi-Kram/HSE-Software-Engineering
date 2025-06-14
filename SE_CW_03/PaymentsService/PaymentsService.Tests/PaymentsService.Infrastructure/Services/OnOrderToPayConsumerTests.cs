using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Infrastructure.Services;
using System.Text.Json;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Services
{
    public class OnOrderToPayConsumerTests
    {
        [Fact]
        public async Task ExecuteAsync_ConsumesMessageAndCallsOnNewOrderToPayAsync()
        {
            // Arrange
            var tcs = new TaskCompletionSource();

            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            var mockProvider = new Mock<IServiceProvider>();
            var mockLogger = new Mock<ILogger<OnOrderToPayConsumer>>();
            var mockConsumer = new Mock<IConsumer<string, string>>();
            var mockAccountService = new Mock<IAccountService>();

            var orderId = Guid.NewGuid();
            var json = JsonSerializer.Serialize(new
            {
                order_id = orderId,
                user_id = 42,
                created_at = DateTime.UtcNow,
                bill = 123.45m
            });

            var message = new Message<string, string> { Value = json };
            var result = new ConsumeResult<string, string>
            {
                Message = message
            };

            mockConsumer.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                        .Callback(() => tcs.TrySetResult())
                        .Returns(result);

            mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);
            mockScope.Setup(s => s.ServiceProvider).Returns(mockProvider.Object);
            mockProvider.Setup(p => p.GetService(typeof(IAccountService))).Returns(mockAccountService.Object);

            var consumerService = new OnOrderToPayConsumer(
                mockScopeFactory.Object,
                mockLogger.Object,
                mockConsumer.Object
            );

            using var cts = new CancellationTokenSource();

            // Act
            var serviceTask = consumerService.StartAsync(cts.Token);

            // Ждём пока вызовется Consume
            await Task.WhenAny(tcs.Task, Task.Delay(5000, cts.Token));
            cts.Cancel(); // Останавливаем BackgroundService

            await consumerService.StopAsync(CancellationToken.None); // Дожидаемся завершения

            // Assert
            mockConsumer.Verify(c => c.Consume(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            mockAccountService.Verify(s => s.OnNewOrderToPayAsync(It.IsAny<OrderToPayMessage>()), Times.AtLeastOnce);
            mockConsumer.Verify(c => c.Commit(It.IsAny<ConsumeResult<string, string>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenDependenciesAreNull()
        {
            var logger = new Mock<ILogger<OnOrderToPayConsumer>>().Object;
            var consumer = new Mock<IConsumer<string, string>>().Object;

            Assert.Throws<ArgumentNullException>(() => new OnOrderToPayConsumer(null!, logger, consumer));
            Assert.Throws<ArgumentNullException>(() => new OnOrderToPayConsumer(Mock.Of<IServiceScopeFactory>(), null!, consumer));
            Assert.Throws<ArgumentNullException>(() => new OnOrderToPayConsumer(Mock.Of<IServiceScopeFactory>(), logger, null!));
        }

        [Fact]
        public void Dispose_CallsConsumerCloseAndDispose()
        {
            // Arrange
            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockLogger = new Mock<ILogger<OnOrderToPayConsumer>>();
            var mockConsumer = new Mock<IConsumer<string, string>>();

            var consumerService = new OnOrderToPayConsumer(
                mockScopeFactory.Object,
                mockLogger.Object,
                mockConsumer.Object
            );

            // Act
            consumerService.Dispose();

            // Assert
            mockConsumer.Verify(c => c.Close(), Times.Once);
            mockConsumer.Verify(c => c.Dispose(), Times.Once);
        }
    }
}
