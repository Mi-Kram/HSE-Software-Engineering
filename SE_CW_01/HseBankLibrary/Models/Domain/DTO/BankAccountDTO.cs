namespace HseBankLibrary.Models.Domain.DTO
{
    public class BankAccountDTO
    {
		public ulong ID { get; set; }
        public string? Name { get; set; }
        public decimal Balance { get; set; }
    }
}
