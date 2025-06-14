using Microsoft.Extensions.DependencyInjection;
using OrderStatusChangeNotifier.Application.UseCases;
using OrderStatusChangeNotifier.Domain.Interfaces;

namespace OrderStatusChangeNotifier.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddSingleton<IWebSocketManager, WebSocketManager>();
        }
    }
}
