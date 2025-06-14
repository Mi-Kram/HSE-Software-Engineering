using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderStatusChangeNotifier.Application.Interfaces;
using OrderStatusChangeNotifier.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace OrderStatusChangeNotifier.Application.UseCases
{
    public class WebSocketManager : IWebSocketManager, IDisposable
    {
        private readonly INewOrderConsumer newOrderConsumer;

        private readonly IPaidOrderConsumer paidOrderConsumer;

        private readonly ConcurrentDictionary<string, WebSocket> connections = new();

        public WebSocketManager(IServiceProvider serviceProvider)
        {
            newOrderConsumer = serviceProvider
                .GetServices<IHostedService>()
                .OfType<INewOrderConsumer>()
                .FirstOrDefault() 
                ?? throw new ArgumentNullException(nameof(INewOrderConsumer), $"Сервис {nameof(INewOrderConsumer)} не запущен");

            paidOrderConsumer = serviceProvider
                .GetServices<IHostedService>()
                .OfType<IPaidOrderConsumer>()
                .FirstOrDefault() 
                ?? throw new ArgumentNullException(nameof(IPaidOrderConsumer), $"Сервис {nameof(IPaidOrderConsumer)} не запущен");

            newOrderConsumer.OnOrdersStatusChangedEvent += OnOrderStatusChanged;
            paidOrderConsumer.OnOrdersStatusChangedEvent += OnOrderStatusChanged;
        }

        public void Dispose()
        {
            newOrderConsumer.OnOrdersStatusChangedEvent -= OnOrderStatusChanged;
            paidOrderConsumer.OnOrdersStatusChangedEvent -= OnOrderStatusChanged;
        }

        public async Task HandleWebSocketConnectionAsync(WebSocket webSocket)
        {
            string key = Guid.NewGuid().ToString();
            while (!connections.TryAdd(key, webSocket)) key = Guid.NewGuid().ToString();

            byte[] buffer = new byte[1024];
            WebSocketReceiveResult result;

            while (webSocket.State == WebSocketState.Open)
            {

                try
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                catch
                {
                    continue;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    connections.TryRemove(key, out _);
                }
            }
        }

        private void OnOrderStatusChanged(string orderID, string status)
        {
            WebSocket[] sockets = [.. connections.Values];
            if (sockets.Length == 0) return;

            byte[] bytes = Encoding.UTF8.GetBytes($"{orderID};{status}");
            List<Task> tasks = new(sockets.Length);

            CancellationTokenSource token = new();
            token.CancelAfter(TimeSpan.FromSeconds(10));

            try
            {
                for (int i = 0; i < sockets.Length; i++)
                {
                    WebSocket socket = sockets[i];

                    if (socket.State == WebSocketState.Closed ||
                        socket.State == WebSocketState.CloseSent ||
                        socket.State == WebSocketState.CloseReceived) continue;

                    tasks.Add(socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, token.Token));
                }

                Task.WhenAll(tasks).Wait();
            }
            catch
            { }

            tasks.ForEach(x => x.Dispose());
        }
    }
}
