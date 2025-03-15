using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Models.Domain
{
    /// <summary>
    /// Операция.
    /// </summary>
    public class Operation : IEntity<string, OperationDTO>
    {
        private string id = string.Empty;
        private decimal amount;
        private string description = "";

        public string ID
        {
            get => id;
            set => id = value ?? throw new ArgumentNullException(nameof(ID));
        }

        public bool IsIncome { get; set; }

        public ulong BankAccountID { get; set; }

        public ulong CategoryID { get; set; }

        public decimal Amount
        {
            get => amount;
            set => amount = value > 0 ? value : throw new InvalidDataException();
        }

        public DateTime Date { get; set; }

        public string Description
        {
            get => description;
            set => description = value ?? throw new ArgumentNullException(nameof(Description));
        }

        public OperationDTO ToDTO()
        {
            return new OperationDTO
            {
                ID = ID,
                IsIncome = IsIncome,
                BankAccountID = BankAccountID,
                CategoryID = CategoryID,
                Amount = Amount,
                Date = Date,
                Description = Description,
            };
        }
    }
}
