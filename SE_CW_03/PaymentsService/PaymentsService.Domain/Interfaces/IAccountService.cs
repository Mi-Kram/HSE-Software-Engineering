using PaymentsService.Domain.Entities;

namespace PaymentsService.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAsync();
        Task<Account> GetAsync(int id);

        Task<int> CreateAccountAsync(string caption, decimal initialBalance);
        Task TopUpBalanceAsync(int id, decimal operation);

        Task OnNewOrderToPayAsync(OrderToPayMessage message);

        Task<OrderToPayMessage?> TryReserveOrderToPayMessageAsync(TimeSpan reserveTime);
        Task PayForOrderAsync(OrderToPayMessage message);

        Task<PaidOrderMessage?> TryReservePaidOrderMessageAsync(TimeSpan reserveTime);
        Task DeletePaidOrderMessageAsync(Guid orderID);
    }
}
