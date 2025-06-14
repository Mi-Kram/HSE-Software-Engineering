namespace PaymentsService.API.DTO
{
    public class CreateAccountDTO
    {
        public string Caption { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}
