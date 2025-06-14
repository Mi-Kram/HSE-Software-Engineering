using PaymentsService.Infrastructure.DTO;

namespace PaymentsService.Tests.PaymentsService.Infrastructure
{
    public class ModelsTests
    {
        [Fact]
        public void OrderIdDTO_Test()
        {
            OrderIdDTO dto = new() { OrderID = Guid.NewGuid() };
            Assert.NotEmpty(dto.OrderID.ToString());
        }
    }
}
