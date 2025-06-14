using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersService.API.Controllers;
using OrdersService.API.DTO;
using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Tests.OrdersService.API
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrdersService> mockOrderService;
        private readonly OrdersController controller;

        public OrdersControllerTests()
        {
            mockOrderService = new Mock<IOrdersService>();
            controller = new OrdersController(mockOrderService.Object);
        }


        [Fact]
        public async Task GetAllOrdersAsync_ReturnsJsonResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order> { new() { ID = Guid.NewGuid(), UserID = 5, Bill = 20, Status = OrderStatus.AwaitForPayment, CreatedAt = DateTime.Now } };
            mockOrderService.Setup(s => s.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task GetAllByUserIDAsync_ReturnsJsonResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order> { new() { ID = Guid.NewGuid(), UserID = 5, Bill = 20, Status = OrderStatus.AwaitForPayment, CreatedAt = DateTime.Now } };
            mockOrderService.Setup(s => s.GetAllByUserIDAsync(It.IsAny<int>())).ReturnsAsync(orders);

            // Act
            var result = await controller.GetAllByUserIDAsync(5);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task GetByIDAsync_ReturnsJsonResult_WithAccount()
        {
            // Arrange
            Order order = new() { ID = Guid.NewGuid(), UserID = 5, Bill = 20, Status = OrderStatus.AwaitForPayment, CreatedAt = DateTime.Now };
            mockOrderService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            // Act
            var result = await controller.GetByIDAsync(Guid.NewGuid());

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task GetStatusByIDAsync_ReturnsJsonResult_WithAccount()
        {
            // Arrange
            Order order = new() { ID = Guid.NewGuid(), UserID = 5, Bill = 20, Status = OrderStatus.AwaitForPayment, CreatedAt = DateTime.Now };
            mockOrderService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);

            // Act
            var result = await controller.GetStatusByIDAsync(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task CreateAccountAsync_WithValidDTO_ReturnsUserId()
        {
            // Arrange
            var dto = new CreateOrderDTO { UserID = 5, Bill = 100 };
            Guid guid = Guid.NewGuid();
            mockOrderService.Setup(s => s.CreateOrderAsync(It.IsAny<OrderDTO>())).ReturnsAsync(guid);

            // Act
            var result = await controller.PostAsync(dto);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(guid, jsonResult.Value?.GetType()?.GetProperty("id")?.GetValue(jsonResult.Value));
        }

        [Fact]
        public async Task CreateAccountAsync_WithNullDTO_ReturnsBadRequest()
        {
            var result = await controller.PostAsync(null!);
            Assert.IsType<BadRequestResult>(result);
        }

    }
}
