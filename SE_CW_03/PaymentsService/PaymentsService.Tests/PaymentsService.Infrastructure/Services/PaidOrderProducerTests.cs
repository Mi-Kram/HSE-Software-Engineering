using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Infrastructure.Services;

namespace PaymentsService.Tests.PaymentsService.Infrastructure.Services
{
    public class PaidOrderProducerTests
    {
        private readonly Mock<IServiceScopeFactory> scopeFactoryMock = new();
        private readonly Mock<IServiceScope> scopeMock = new();
        private readonly Mock<IServiceProvider> providerMock = new();
        private readonly Mock<IAccountService> accountServiceMock = new();
        private readonly Mock<IProducer<string, string>> producerMock = new();
        private readonly Mock<ILogger<PaidOrderProducer>> loggerMock = new();

        private const string Topic = "test-topic";
        private readonly TimeSpan reserveTime = TimeSpan.FromSeconds(5);

        public PaidOrderProducerTests()
        {
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);
            scopeMock.Setup(s => s.ServiceProvider).Returns(providerMock.Object);
            providerMock.Setup(p => p.GetService(typeof(IAccountService)))
                        .Returns(accountServiceMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenServiceScopeFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PaidOrderProducer(null!, loggerMock.Object, producerMock.Object, Topic, reserveTime));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenLoggerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PaidOrderProducer(scopeFactoryMock.Object, null!, producerMock.Object, Topic, reserveTime));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenProducerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PaidOrderProducer(scopeFactoryMock.Object, loggerMock.Object, null!, Topic, reserveTime));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenTopicIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PaidOrderProducer(scopeFactoryMock.Object, loggerMock.Object, producerMock.Object, null!, reserveTime));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenReserveTimeIsZero()
        {
            Assert.Throws<ArgumentException>(() =>
                new PaidOrderProducer(scopeFactoryMock.Object, loggerMock.Object, producerMock.Object, Topic, TimeSpan.Zero));
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenNoMessage()
        {
            // Arrange
            var producer = CreateProducer();
            accountServiceMock.Setup(s => s.TryReservePaidOrderMessageAsync(reserveTime))
                              .ReturnsAsync((PaidOrderMessage?)null);

            // Act
            var result = await InvokeHandleAsync(producer);

            // Assert
            Assert.False(result);
            producerMock.Verify(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()), Times.Never);
            accountServiceMock.Verify(s => s.DeletePaidOrderMessageAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendAndDelete_WhenMessageIsAvailable()
        {
            // Arrange
            var msg = new PaidOrderMessage { OrderID = Guid.NewGuid(), Payload = "payload" };
            accountServiceMock.Setup(s => s.TryReservePaidOrderMessageAsync(reserveTime))
                              .ReturnsAsync(msg);

            producerMock.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new DeliveryResult<string, string>());

            var producer = CreateProducer();

            // Act
            var result = await InvokeHandleAsync(producer);

            // Assert
            Assert.True(result);
            producerMock.Verify(p => p.ProduceAsync(Topic, It.Is<Message<string, string>>(m => m.Value == msg.Payload), It.IsAny<CancellationToken>()), Times.Once);
            accountServiceMock.Verify(s => s.DeletePaidOrderMessageAsync(msg.OrderID), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldCallProducerDispose()
        {
            var producer = CreateProducer();
            producer.Dispose();
            producerMock.Verify(p => p.Dispose(), Times.Once);
        }

        // Helper method to call private HandleAsync
        private static async Task<bool> InvokeHandleAsync(PaidOrderProducer producer)
        {
            var method = typeof(PaidOrderProducer).GetMethod("HandleAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<bool>)method!.Invoke(producer, new object[] { CancellationToken.None })!;
            return await task;
        }

        private PaidOrderProducer CreateProducer()
        {
            return new PaidOrderProducer(
                scopeFactoryMock.Object,
                loggerMock.Object,
                producerMock.Object,
                Topic,
                reserveTime);
        }
    }
}
