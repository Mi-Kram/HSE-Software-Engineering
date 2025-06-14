using Microsoft.AspNetCore.Mvc;

namespace GatewayService.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        // api/orders
        [HttpGet]
        public Task GetAllAsync()
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/orders/by-user/5
        [HttpGet("by-user/{user_id}")]
        public Task GetAllByUserIDAsync(int _)
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/orders/<guid>
        [HttpGet("{id}")]
        public Task GetByIDAsync(Guid id)
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/orders/<guid>/status
        [HttpGet("{id}/status")]
        public Task GetStatusByIDAsync(Guid _)
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }

        // api/orders
        [HttpPost]
        public Task PostAsync()
        {
            throw new NotImplementedException("Should be redirected by Yarp.ReverseProxy");
        }
    }
}
