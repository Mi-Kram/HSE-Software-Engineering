namespace OrdersService.Domain.Exceptions
{
    public class OrderNotExistsException(Guid orderID) 
        : ArgumentException($"Заказ с id {orderID} не найден")
    { }
}
