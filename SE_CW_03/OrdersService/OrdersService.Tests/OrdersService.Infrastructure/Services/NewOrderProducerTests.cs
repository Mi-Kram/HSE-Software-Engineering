using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;
using OrdersService.Infrastructure.Services;

namespace OrdersService.Tests.OrdersService.Infrastructure.Services
{
    public class NewOrderProducerTests
    {
        [Fact]
        public async Task NewOrderProducer_ProcessesMessageOnce_AndStops()
        {
            // Arrange
            var message = new NewOrderMessage
            {
                OrderID = Guid.NewGuid(),
                UserID = 42,
                Payload = "{\"order_id\":\"test\"}",
                Reserved = DateTime.Now
            };

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.SetupSequence(s => s.TryReserveNewOrderMessageAsync(It.IsAny<TimeSpan>()))
                             .ReturnsAsync(message)
                             .ReturnsAsync((NewOrderMessage?)null); // затем вернёт null, чтобы выйти из цикла

            ordersServiceMock.Setup(s => s.AwaitForPaymentAsync(message.OrderID))
                             .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IOrdersService)))
                               .Returns(ordersServiceMock.Object);

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            var producerMock = new Mock<IProducer<string, string>>();
            producerMock.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new DeliveryResult<string, string>());

            var loggerMock = new Mock<ILogger<NewOrderProducer>>();

            var service = new NewOrderProducer(scopeFactoryMock.Object, loggerMock.Object, producerMock.Object, "test-topic", TimeSpan.FromSeconds(5));

            var cts = new CancellationTokenSource();

            // Act
            var serviceTask = service.StartAsync(cts.Token);

            // ждём первую итерацию и отменяем токен
            await Task.Delay(1000);
            await service.StopAsync(CancellationToken.None);

            // Assert
            ordersServiceMock.Verify(s => s.TryReserveNewOrderMessageAsync(It.IsAny<TimeSpan>()), Times.AtLeastOnce);
            producerMock.Verify(p => p.ProduceAsync("test-topic",
                It.Is<Message<string, string>>(m => m.Key == "42" && m.Value == message.Payload),
                It.IsAny<CancellationToken>()), Times.Once);
            ordersServiceMock.Verify(s => s.AwaitForPaymentAsync(message.OrderID), Times.Once);
        }
    }
}
