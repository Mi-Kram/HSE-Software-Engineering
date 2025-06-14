using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Exceptions;
using OrdersService.Domain.Models;

namespace OrdersService.Tests.OrdersService.Domain
{
    public class ModelsTests
    {
        [Fact]
        public void OrderDTO_Test()
        {
            DateTime time = DateTime.Now;
            OrderDTO dto = new() { UserID = 5, Bill = 20, CreatedAt = time };

            Assert.Equal(5, dto.UserID);
            Assert.Equal(20, dto.Bill);
            Assert.Equal(time, dto.CreatedAt);
        }

        [Fact]
        public void ApplicationVariables_Test()
        {
            Assert.NotNull(ApplicationVariables.DB_CONNECTION);
            Assert.NotEmpty(ApplicationVariables.DB_CONNECTION);

            Assert.NotNull(ApplicationVariables.KAFKA);
            Assert.NotEmpty(ApplicationVariables.KAFKA);

            Assert.NotNull(ApplicationVariables.TOPIC_NEW_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_NEW_ORDER);

            Assert.NotNull(ApplicationVariables.TOPIC_PAID_ORDER);
            Assert.NotEmpty(ApplicationVariables.TOPIC_PAID_ORDER);
        }

        [Fact]
        public void EnvVariableException_Test()
        {
            EnvVariableException ex = new("Test");
            Assert.Equal("Test: переменная среды не найдена", ex.Message);
            Assert.Equal("Test", ex.EnvName);

            Assert.Throws<ArgumentNullException>(() => new EnvVariableException(null!));
        }

        [Fact]
        public void OrderNotExistsException_Test()
        {
            Guid guid = Guid.NewGuid();
            OrderNotExistsException ex = new(guid);
            Assert.Equal($"Заказ с id {guid} не найден", ex.Message);
        }

        [Fact]
        public void NewOrderMessage_Test()
        {
            Guid guid = Guid.NewGuid();
            DateTime dt1 = DateTime.Now, dt2 = DateTime.Now.AddMinutes(1);

            NewOrderMessage message = new()
            {
                OrderID = guid,
                UserID = 5,
                Payload = "Test",
                CreatedAt = dt1,
                Reserved = dt2
            };

            Assert.Equal(guid, message.OrderID);
            Assert.Equal(5, message.UserID);
            Assert.Equal("Test", message.Payload);
            Assert.Equal(dt1, message.CreatedAt);
            Assert.Equal(dt2, message.Reserved);

            Assert.Throws<ArgumentNullException>(() => message.Payload = null!);
        }

        [Fact]
        public void PaidOrderMessage_Test()
        {
            Guid guid = Guid.NewGuid();
            DateTime dt = DateTime.Now;

            PaidOrderMessage message = new()
            {
                OrderID = guid,
                Status = OrderStatus.Completed,
                Reserved = dt
            };

            Assert.Equal(guid, message.OrderID);
            Assert.Equal(OrderStatus.Completed, message.Status);
            Assert.Equal(dt, message.Reserved);
        }

        [Fact]
        public void Order_Test()
        {
            Guid guid = Guid.NewGuid();
            DateTime dt = DateTime.Now;

            Order order = new()
            {
                ID = guid,
                UserID = 5,
                Bill = 20,
                CreatedAt = dt,
                Status = OrderStatus.NewOrder
            };

            Assert.Equal(guid, order.ID);
            Assert.Equal(5, order.UserID);
            Assert.Equal(20, order.Bill);
            Assert.Equal(dt, order.CreatedAt);
            Assert.Equal(OrderStatus.NewOrder, order.Status);
        }
    }
}
