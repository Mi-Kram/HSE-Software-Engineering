namespace PaymentsService.Domain.Models
{
    public enum OrderStatus
    {
        Completed,
        CanceledNoUserFound,
        CanceledNoFunds
    }
}
