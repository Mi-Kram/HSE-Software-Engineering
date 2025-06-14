using System.Net.WebSockets;

namespace OrderStatusChangeNotifier.Domain.Interfaces
{
    public interface IWebSocketManager
    {
        Task HandleWebSocketConnectionAsync(WebSocket webSocket);
    }
}
