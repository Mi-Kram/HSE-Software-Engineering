using Microsoft.AspNetCore.Http;

namespace GatewayService.Domain.Interfaces
{
    public interface IOrdersService
    {
        Task GetAllOrdersAsync(HttpContext httpContext);
        Task GetAllOrdersByUserIDAsync(HttpContext httpContext, int userID);
        Task GetOrderAsync(HttpContext httpContext, Guid orderID);
        Task GetOrderStatusAsync(HttpContext httpContext, Guid orderID);
        Task CreateOrderAsync(HttpContext httpContext);
    }
}
