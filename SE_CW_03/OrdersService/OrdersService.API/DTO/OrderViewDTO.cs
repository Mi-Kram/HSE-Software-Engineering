using OrdersService.Domain.Entities;

namespace OrdersService.API.DTO
{
    public class OrderViewDTO(Order order)
    {
        public Guid ID { get; } = order.ID;
        public int UserID { get; } = order.UserID;
        public decimal Bill { get; } = order.Bill;
        public DateTime CreatedAt { get; } = order.CreatedAt;
        public string Status { get; } = order.Status.ToString();
    }
}
