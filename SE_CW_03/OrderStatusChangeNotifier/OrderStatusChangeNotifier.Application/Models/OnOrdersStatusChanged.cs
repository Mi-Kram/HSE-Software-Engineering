namespace OrderStatusChangeNotifier.Application.Models
{
    public delegate void OnOrdersStatusChangedDelegete(string orderID, string status);
}
