namespace OrdersService.Domain.Entities
{
    public enum OrderStatus
    {
        NewOrder,
        AwaitForPayment,
        Completed,
        CanceledNoUserFound,
        CanceledNoFunds
    }
}
