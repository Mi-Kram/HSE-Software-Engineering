using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Exceptions;
using OrdersService.Domain.Interfaces.MessagesRepository;
using OrdersService.Domain.Interfaces;

using ApplicationOrdersService = OrdersService.Application.UseCases.OrdersService;

namespace OrdersService.Tests.OrdersService.Application
{
    public class OrdersServiceTests
    {
        private readonly Mock<IOrdersRepository> mockOrderRepo = new();
        private readonly Mock<INewOrderMessagesRepository> mockNewOrderRepo = new();
        private readonly Mock<IPaidOrderMessagesRepository> mockPaidOrderRepo = new();
        private readonly Mock<IDbTransaction> mockTransaction = new();
        private readonly Mock<ILogger<ApplicationOrdersService>> mockLogger = new();

        private ApplicationOrdersService CreateService()
        {
            return new ApplicationOrdersService(
                mockOrderRepo.Object,
                mockNewOrderRepo.Object,
                mockPaidOrderRepo.Object,
                mockTransaction.Object,
                mockLogger.Object
            );
        }

        [Fact]
        public async Task GetAsync_ExistingOrder_ReturnsOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var expectedOrder = new Order { ID = orderId };
            mockOrderRepo.Setup(r => r.GetAsync(orderId)).ReturnsAsync(expectedOrder);

            var service = CreateService();

            // Act
            var result = await service.GetAsync(orderId);

            // Assert
            Assert.Equal(expectedOrder, result);
        }

        [Fact]
        public async Task GetAsync_NotFound_ThrowsException()
        {
            var orderId = Guid.NewGuid();
            mockOrderRepo.Setup(r => r.GetAsync(orderId)).ReturnsAsync((Order?)null);
            var service = CreateService();

            await Assert.ThrowsAsync<OrderNotExistsException>(() => service.GetAsync(orderId));
        }

        [Fact]
        public async Task CreateOrderAsync_ValidDto_SavesOrderAndMessage()
        {
            // Arrange
            var dto = new OrderDTO { UserID = 1, Bill = 100 };
            var order = new Order
            {
                ID = Guid.NewGuid(),
                UserID = dto.UserID,
                CreatedAt = DateTime.UtcNow,
                Bill = dto.Bill
            };

            mockOrderRepo.Setup(r => r.AddOrderAsync(dto)).ReturnsAsync(order);

            var service = CreateService();

            // Act
            var result = await service.CreateOrderAsync(dto);

            // Assert
            Assert.Equal(order.ID, result);
            mockTransaction.Verify(t => t.BeginTransactionAsync(), Times.Once);
            mockNewOrderRepo.Verify(m => m.AddMessageAsync(It.Is<NewOrderMessage>(msg => msg.OrderID == order.ID)), Times.Once);
            mockTransaction.Verify(t => t.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsync_ValidStatus_UpdatesAndDeletes()
        {
            // Arrange
            var message = new PaidOrderMessage
            {
                OrderID = Guid.NewGuid(),
                Status = OrderStatus.Completed
            };

            var service = CreateService();

            // Act
            await service.CompleteOrderAsync(message);

            // Assert
            mockOrderRepo.Verify(r => r.SetStatusAsync(message.OrderID, message.Status), Times.Once);
            mockPaidOrderRepo.Verify(r => r.DeleteMessageAsync(message.OrderID), Times.Once);
        }

        [Fact]
        public async Task CompleteOrderAsync_InvalidStatus_LogsError()
        {
            var message = new PaidOrderMessage
            {
                OrderID = Guid.NewGuid(),
                Status = OrderStatus.NewOrder // Недопустимый для завершения
            };

            var service = CreateService();

            await service.CompleteOrderAsync(message);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("неверный статус")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            mockOrderRepo.Verify(r => r.SetStatusAsync(It.IsAny<Guid>(), It.IsAny<OrderStatus>()), Times.Never);
            mockPaidOrderRepo.Verify(r => r.DeleteMessageAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
