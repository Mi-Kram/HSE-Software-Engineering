using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Infrastructure.Persistence
{
    public class AccountRepository(AppDbContext context) : IAccountRepository
    {
        private readonly AppDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<List<Account>> GetAllAsync()
        {
            return await context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetAsync(int id)
        {
            return await context.Accounts.FindAsync(id);
        }

        public async Task<int> CreateAccountAsync(Account account)
        {
            ArgumentNullException.ThrowIfNull(account, nameof(account));

            EntityEntry<Account> res = await context.AddAsync(account);
            await context.SaveChangesAsync();

            return res.Entity.UserID;
        }

        public async Task<bool> ApplyOperationAsync(int id, decimal operation)
        {
            bool result = await context.Accounts
                .Where(x => x.UserID == id)
                .ExecuteUpdateAsync(x =>
                x.SetProperty(p => p.Balance, p => p.Balance + operation)) != 0;

            if (result) await context.SaveChangesAsync();
            return result;
        }
    }
}
