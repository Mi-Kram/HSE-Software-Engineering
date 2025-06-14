using OrdersService.Domain.Entities;

namespace OrdersService.Domain.Interfaces.MessagesRepository
{
    public interface IPaidOrderMessagesRepository
    {
        Task<PaidOrderMessage?> GetMessageAsync(Guid orderID);
        Task AddMessageAsync(PaidOrderMessage message);
        Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime);
        Task DeleteMessageAsync(Guid orderID);
    }
}
