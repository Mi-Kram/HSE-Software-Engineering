using PaymentsService.Domain.Entities;

namespace PaymentsService.Domain.Interfaces.MessagesRepository
{
    public interface IOrderToPayMessagesRepository
    {
        Task<OrderToPayMessage?> GetMessageAsync(Guid orderID);
        Task AddMessageAsync(OrderToPayMessage message);
        Task DeleteMessageAsync(Guid orderID);
        Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime);
    }
}
