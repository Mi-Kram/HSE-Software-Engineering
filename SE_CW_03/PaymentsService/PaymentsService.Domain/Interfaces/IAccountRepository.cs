using PaymentsService.Domain.Entities;

namespace PaymentsService.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetAsync(int id);

        Task<int> CreateAccountAsync(Account account);
        Task<bool> ApplyOperationAsync(int id, decimal operation);
    }
}
