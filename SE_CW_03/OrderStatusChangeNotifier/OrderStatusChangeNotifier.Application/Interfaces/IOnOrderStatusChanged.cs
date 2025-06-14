using OrderStatusChangeNotifier.Application.Models;

namespace OrderStatusChangeNotifier.Application.Interfaces
{
    public interface IOnOrderStatusChanged
    {
        event OnOrdersStatusChangedDelegete? OnOrdersStatusChangedEvent;
    }
}
