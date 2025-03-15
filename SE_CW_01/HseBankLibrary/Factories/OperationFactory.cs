using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;

namespace HseBankLibrary.Factories
{
    /// <summary>
    /// Фабрика для создания операции.
    /// </summary>
    public class OperationFactory : IDomainFactory<Operation, OperationDTO>
    {
        public Operation Create(OperationDTO args)
        {
            ArgumentNullException.ThrowIfNull(args);

            return new Operation()
            {
                ID = args.ID ?? string.Empty,
                IsIncome = args.IsIncome,
                BankAccountID = args.BankAccountID,
                CategoryID = args.CategoryID,
                Amount = args.Amount,
                Date = args.Date,
                Description = args.Description ?? string.Empty,
            };
        }
    }
}
