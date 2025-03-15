using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Models.Domain
{
    /// <summary>
    /// Счёт.
    /// </summary>
    public class BankAccount : IEntity<ulong, BankAccountDTO>
    {
        private string name = "";

        public ulong ID { get; set; }

        public string Name
		{
			get => name;
			set => name = value ?? throw new ArgumentNullException(nameof(Name));
		}

        public decimal Balance { get; set; } = 0;

        public BankAccountDTO ToDTO()
        {
            return new BankAccountDTO
            {
                ID = ID,
                Name = Name,
                Balance = Balance
            };
        }
    }
}
