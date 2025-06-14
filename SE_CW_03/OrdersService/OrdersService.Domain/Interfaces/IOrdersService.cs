using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;

namespace OrdersService.Domain.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetAllByUserIDAsync(int userID);
        Task<Order> GetAsync(Guid orderID);

        Task<Guid> CreateOrderAsync(OrderDTO dto);


        Task<NewOrderMessage?> TryReserveNewOrderMessageAsync(TimeSpan reserveTime);
        Task AwaitForPaymentAsync(Guid orderID);

        Task OnPaymentResultReceivedAsync(Guid orderID, OrderStatus status);

        Task<PaidOrderMessage?> TryReservePaidOrderMessageAsync(TimeSpan reserveTime);
        Task CompleteOrderAsync(PaidOrderMessage message);
    }
}
