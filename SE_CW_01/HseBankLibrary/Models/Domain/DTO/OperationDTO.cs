namespace HseBankLibrary.Models.Domain.DTO
{
    public class OperationDTO
    {
        public string? ID { get; set; }
        public bool IsIncome { get; set; }
        public ulong BankAccountID { get; set; }
        public ulong CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
