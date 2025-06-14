using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;

namespace OrdersService.Domain.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetAllByUserIDAsync(int userID);
        Task<Order?> GetAsync(Guid orderID);

        Task<Order> AddOrderAsync(OrderDTO dto);
        Task SetStatusAsync(Guid orderID, OrderStatus status);
    }
}
