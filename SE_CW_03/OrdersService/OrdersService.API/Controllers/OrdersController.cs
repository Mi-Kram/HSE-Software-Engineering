using Microsoft.AspNetCore.Mvc;
using OrdersService.API.DTO;
using OrdersService.Domain.DTO;
using OrdersService.Domain.Interfaces;

namespace OrdersService.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(IOrdersService service) : ControllerBase
    {
        private readonly IOrdersService service = service ?? throw new ArgumentNullException(nameof(service));

        // api/orders
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return new JsonResult((await service.GetAllAsync()).Select(x => new OrderViewDTO(x)));
        }

        // api/orders/by-user/5
        [HttpGet("by-user/{user_id}")]
        public async Task<IActionResult> GetAllByUserIDAsync([FromRoute(Name = "user_id")] int userID)
        {
            return new JsonResult((await service.GetAllByUserIDAsync(userID)).Select(x => new OrderViewDTO(x)));
        }

        // api/orders/<guid>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync(Guid id)
        {
            return new JsonResult(new OrderViewDTO(await service.GetAsync(id)));
        }

        // api/orders/<guid>/status
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatusByIDAsync(Guid id)
        {
            return Ok((await service.GetAsync(id)).Status.ToString());
        }

        // api/orders
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] CreateOrderDTO dto)
        {
            if (dto == null) return BadRequest();

            Guid id = await service.CreateOrderAsync(new OrderDTO
            {
                UserID = dto.UserID,
                Bill = dto.Bill,
                CreatedAt = DateTime.Now
            });

            return new JsonResult(new
            {
                id
            });
        }
    }
}
