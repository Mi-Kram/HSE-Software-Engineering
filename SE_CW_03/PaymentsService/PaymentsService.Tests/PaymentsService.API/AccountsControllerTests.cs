using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentsService.API.Controllers;
using PaymentsService.API.DTO;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Tests.PaymentsService.API
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> mockAccountService;
        private readonly AccountsController controller;

        public AccountsControllerTests()
        {
            mockAccountService = new Mock<IAccountService>();
            controller = new AccountsController(mockAccountService.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsJsonResult_WithAccounts()
        {
            // Arrange
            var accounts = new List<Account> { new() { UserID = 1, Caption = "Test", Balance = 100 } };
            mockAccountService.Setup(s => s.GetAllAsync()).ReturnsAsync(accounts);

            // Act
            var result = await controller.GetAllAccountsAsync();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(accounts, jsonResult.Value);
        }

        [Fact]
        public async Task GetAccountsAsync_ReturnsJsonResult_WithAccount()
        {
            // Arrange
            var account = new Account { UserID = 5, Caption = "User5", Balance = 200 };
            mockAccountService.Setup(s => s.GetAsync(5)).ReturnsAsync(account);

            // Act
            var result = await controller.GetAccountsAsync(5);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(account, jsonResult.Value);
        }

        [Fact]
        public async Task CreateAccountAsync_WithValidDTO_ReturnsUserId()
        {
            // Arrange
            var dto = new CreateAccountDTO { Caption = "NewUser", InitialBalance = 500m };
            mockAccountService.Setup(s => s.CreateAccountAsync(It.IsAny<string>(), It.IsAny<decimal>()))
                              .ReturnsAsync(42);

            // Act
            var result = await controller.CreateAccountAsync(dto);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(42, jsonResult.Value?.GetType()?.GetProperty("user_id")?.GetValue(jsonResult.Value));
        }

        [Fact]
        public async Task CreateAccountAsync_WithNullDTO_ReturnsBadRequest()
        {
            var result = await controller.CreateAccountAsync(null!);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task TopUpBalanceAsync_WithValidDTO_ReturnsNoContent()
        {
            // Arrange
            var dto = new BalanceTopUpDTO { UserID = 1, Operation = 250m };

            // Act
            var result = await controller.TopUpBalanceAsync(dto);

            // Assert
            mockAccountService.Verify(s => s.TopUpBalanceAsync(dto.UserID, dto.Operation), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task TopUpBalanceAsync_WithNullDTO_ReturnsBadRequest()
        {
            // Act
            var result = await controller.TopUpBalanceAsync(null!);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
