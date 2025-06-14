using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;
using OrdersService.Infrastructure.Services;
using System.Text.Json;

namespace OrdersService.Tests.OrdersService.Infrastructure.Services
{
    public class OnPaidOrderResultConsumerTests
    {
        [Fact]
        public async Task OnPaidOrderResultConsumer_ProcessesMessageOnce_AndStops()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var json = JsonSerializer.Serialize(new
            {
                order_id = orderId,
                status = OrderStatus.Completed.ToString()
            });

            var consumeResult = new ConsumeResult<string, string>
            {
                Message = new Message<string, string>
                {
                    Key = "some-key",
                    Value = json
                },
                TopicPartitionOffset = new TopicPartitionOffset("topic", 0, 0)
            };

            var consumerMock = new Mock<IConsumer<string, string>>();
            consumerMock.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                        .Returns(consumeResult);
            consumerMock.Setup(c => c.Commit(It.IsAny<ConsumeResult<string, string>>()));

            var ordersServiceMock = new Mock<IOrdersService>();
            ordersServiceMock.Setup(s => s.OnPaymentResultReceivedAsync(orderId, OrderStatus.Completed))
                             .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IOrdersService)))
                               .Returns(ordersServiceMock.Object);

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            var loggerMock = new Mock<ILogger<OnPaidOrderResultConsumer>>();

            var service = new OnPaidOrderResultConsumer(scopeFactoryMock.Object, loggerMock.Object, consumerMock.Object);

            using var cts = new CancellationTokenSource();

            // ограничим выполнение одной итерацией: вызов Cancel после задержки
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                cts.Cancel();
            });

            // Act
            await service.StartAsync(cts.Token);
            await service.StopAsync(CancellationToken.None);

            // Assert
            ordersServiceMock.Verify(s => s.OnPaymentResultReceivedAsync(orderId, OrderStatus.Completed), Times.Once);
            consumerMock.Verify(c => c.Commit(It.IsAny<ConsumeResult<string, string>>()), Times.Once);
        }
    }
}
