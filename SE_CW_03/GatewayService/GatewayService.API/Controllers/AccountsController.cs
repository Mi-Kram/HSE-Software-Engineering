using Microsoft.AspNetCore.Mvc;

namespace GatewayService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // api/accounts
        [HttpGet]
        public Task GetAllAccountsAsync()
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/accounts/5
        [HttpGet("{id}")]
        public Task GetAccountsAsync(int _)
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/accounts
        [HttpPost]
        public Task CreateAccountAsync()
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/accounts/top-up
        [HttpPatch("top-up")]
        public Task TopUpBalanceAsync()
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }
    }
}
