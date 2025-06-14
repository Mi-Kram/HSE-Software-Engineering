using Microsoft.AspNetCore.Http;

namespace GatewayService.Domain.Interfaces
{
    public interface IAccountsService
    {
        Task GetAllAccountsAsync(HttpContext httpContext);
        Task GetAccountAsync(HttpContext httpContext, int userID);
        Task CreateAccountAsync(HttpContext httpContext);
        Task TopUpBalanceAsync(HttpContext httpContext);
    }
}
