using PaymentsService.Domain.Entities;

namespace PaymentsService.Domain.Interfaces.MessagesRepository
{
    public interface IPaidOrderMessagesRepository
    {
        Task<PaidOrderMessage?> GetMessageAsync(Guid orderID);
        Task AddMessageAsync(PaidOrderMessage message);
        Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime);
        Task DeleteMessageAsync(Guid orderID);
    }
}
