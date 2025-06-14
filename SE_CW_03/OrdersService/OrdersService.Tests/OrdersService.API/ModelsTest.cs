using OrdersService.API.DTO;
using OrdersService.Domain.Entities;

namespace OrdersService.Tests.OrdersService.API
{
    public class ModelsTest
    {
        [Fact]
        public void CreateOrderDTO_Test()
        {
            CreateOrderDTO dto = new() { UserID = 5, Bill = 20 };

            Assert.Equal(5, dto.UserID);
            Assert.Equal(20, dto.Bill);

            Assert.Throws<ArgumentException>(() => dto.Bill = -20);
        }

        [Fact]
        public void OrderViewDTO_Test()
        {
            Guid guid = Guid.NewGuid();
            Order order = new() { ID = guid, UserID = 5, Status = OrderStatus.AwaitForPayment, Bill = 50, CreatedAt = DateTime.Now };
            
            OrderViewDTO dto = new(order);

            Assert.Equal(order.ID, dto.ID);
            Assert.Equal(order.UserID, dto.UserID);
            Assert.Equal(order.Status.ToString(), dto.Status);
            Assert.Equal(order.Bill, dto.Bill);
            Assert.Equal(order.CreatedAt, dto.CreatedAt);
        }
    }
}
