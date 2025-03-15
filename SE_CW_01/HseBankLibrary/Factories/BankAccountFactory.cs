using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;

namespace HseBankLibrary.Factories
{
    /// <summary>
    /// Фабрика для создания счёта.
    /// </summary>
    public class BankAccountFactory : IDomainFactory<BankAccount, BankAccountDTO>
    {
        public BankAccount Create(BankAccountDTO args)
        {
            ArgumentNullException.ThrowIfNull(args);

            return new BankAccount()
            {
                ID = args.ID,
                Name = args.Name ?? string.Empty,
                Balance = args.Balance
            };
        }
    }
}
