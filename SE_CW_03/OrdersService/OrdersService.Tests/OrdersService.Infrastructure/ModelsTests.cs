using OrdersService.Infrastructure.DTO;

namespace OrdersService.Tests.OrdersService.Infrastructure
{
    public class ModelsTests
    {
        [Fact]
        public void OrderIdDTO_Test()
        {
            Guid guid = Guid.NewGuid();
            OrderIdDTO dto = new() { OrderID = guid };
            Assert.Equal(guid, dto.OrderID);
        }
    }
}
