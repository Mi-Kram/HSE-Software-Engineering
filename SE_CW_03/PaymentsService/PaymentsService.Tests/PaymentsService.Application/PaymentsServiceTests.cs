using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;

using ApplicationPaymentsService = PaymentsService.Application.UseCases.PaymentsService;

namespace PaymentsService.Tests.PaymentsService.Application
{
    public class PaymentsServiceTests
    {
        private readonly Mock<IServiceScopeFactory> scopeFactoryMock = new();
        private readonly Mock<IServiceScope> scopeMock = new();
        private readonly Mock<IServiceProvider> serviceProviderMock = new();
        private readonly Mock<IAccountService> accountServiceMock = new();
        private readonly Mock<ILogger<ApplicationPaymentsService>> loggerMock = new();

        private ApplicationPaymentsService CreateService(TimeSpan reserveTime)
        {
            scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
            serviceProviderMock.Setup(x => x.GetService(typeof(IAccountService)))
                .Returns(accountServiceMock.Object);

            return new ApplicationPaymentsService(scopeFactoryMock.Object, loggerMock.Object, reserveTime);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenScopeFactoryNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ApplicationPaymentsService(null!, loggerMock.Object, TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenLoggerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ApplicationPaymentsService(scopeFactoryMock.Object, null!, TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenReserveTimeInvalid()
        {
            Assert.Throws<ArgumentException>(() => new ApplicationPaymentsService(scopeFactoryMock.Object, loggerMock.Object, TimeSpan.Zero));
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenNoMessage()
        {
            // Arrange
            accountServiceMock.Setup(x => x.TryReserveOrderToPayMessageAsync(It.IsAny<TimeSpan>()))
                .ReturnsAsync((OrderToPayMessage?)null);

            var service = CreateService(TimeSpan.FromSeconds(1));
            var handleAsyncMethod = typeof(ApplicationPaymentsService)
                .GetMethod("HandleAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

            // Act
            bool result = await (Task<bool>)handleAsyncMethod.Invoke(service, null)!;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task HandleAsync_ShouldProcessMessage_WhenMessageExists()
        {
            // Arrange
            var msg = new OrderToPayMessage { OrderID = Guid.NewGuid(), Bill = 10, UserID = 1 };
            accountServiceMock.Setup(x => x.TryReserveOrderToPayMessageAsync(It.IsAny<TimeSpan>()))
                .ReturnsAsync(msg);

            var service = CreateService(TimeSpan.FromSeconds(1));
            var handleAsyncMethod = typeof(ApplicationPaymentsService)
                .GetMethod("HandleAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

            // Act
            bool result = await (Task<bool>)handleAsyncMethod.Invoke(service, null)!;

            // Assert
            Assert.True(result);
            accountServiceMock.Verify(x => x.PayForOrderAsync(msg), Times.Once);
        }
    }
}
