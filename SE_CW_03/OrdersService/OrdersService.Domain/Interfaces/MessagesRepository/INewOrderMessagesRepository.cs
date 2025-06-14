using OrdersService.Domain.Entities;

namespace OrdersService.Domain.Interfaces.MessagesRepository
{
    public interface INewOrderMessagesRepository
    {
        Task<NewOrderMessage?> GetMessageAsync(Guid orderID);
        Task AddMessageAsync(NewOrderMessage message);
        Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime);
        Task DeleteMessageAsync(Guid orderID);
    }
}
