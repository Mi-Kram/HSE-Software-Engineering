using Microsoft.AspNetCore.Mvc;
using PaymentsService.API.DTO;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

        // api/accounts
        [HttpGet]
        public async Task<IActionResult> GetAllAccountsAsync()
        {
            return new JsonResult(await accountService.GetAllAsync());
        }

        // api/accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountsAsync(int id)
        {
            return new JsonResult(await accountService.GetAsync(id));
        }

        // api/accounts
        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromForm] CreateAccountDTO dto)
        {
            if (dto == null) return BadRequest();

            return new JsonResult(new
            {
                user_id = await accountService.CreateAccountAsync(dto.Caption, dto.InitialBalance)
            });
        }

        // api/accounts/top-up
        [HttpPatch("top-up")]
        public async Task<IActionResult> TopUpBalanceAsync([FromForm] BalanceTopUpDTO dto)
        {
            if (dto == null) return BadRequest();

            await accountService.TopUpBalanceAsync(dto.UserID, dto.Operation);
            return NoContent();
        }
    }
}
