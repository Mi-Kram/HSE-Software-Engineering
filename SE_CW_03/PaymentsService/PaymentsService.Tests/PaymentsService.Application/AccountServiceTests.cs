using Microsoft.Extensions.Logging;
using Moq;
using PaymentsService.Application.UseCases;
using PaymentsService.Domain.Interfaces.MessagesRepository;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Exceptions;
using System.Text.Json;

namespace PaymentsService.Tests.PaymentsService.Application
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<IOrderToPayMessagesRepository> _orderRepo = new();
        private readonly Mock<IPaidOrderMessagesRepository> _paidRepo = new();
        private readonly Mock<IDbTransaction> _dbTransaction = new();
        private readonly Mock<ILogger<AccountService>> _logger = new();

        private AccountService CreateService() =>
            new(_accountRepository.Object,
                _orderRepo.Object,
                _paidRepo.Object,
                _dbTransaction.Object,
                _logger.Object);
        
        [Fact]
        public async Task GetAllAsync_ReturnsAccounts()
        {
            var expected = new List<Account> { new() { UserID = 1 } };
            _accountRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            var service = CreateService();
            var result = await service.GetAllAsync();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAsync_ExistingId_ReturnsAccount()
        {
            var account = new Account { UserID = 1 };
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(account);

            var service = CreateService();
            var result = await service.GetAsync(1);
            Assert.Equal(account, result);
        }

        [Fact]
        public async Task GetAsync_NotExistingId_Throws()
        {
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync((Account?)null);
            var service = CreateService();
            await Assert.ThrowsAsync<AccountNotExistsException>(() => service.GetAsync(1));
        }

        [Fact]
        public async Task TopUpBalanceAsync_ValidOperation_CallsRepository()
        {
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Account());
            _accountRepository.Setup(r => r.ApplyOperationAsync(1, 100)).ReturnsAsync(true);

            var service = CreateService();
            await service.TopUpBalanceAsync(1, 100);

            _accountRepository.Verify(r => r.ApplyOperationAsync(1, 100), Times.Once);
        }

        [Fact]
        public async Task TopUpBalanceAsync_InvalidAmount_Throws()
        {
            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => service.TopUpBalanceAsync(1, 0));
        }

        [Fact]
        public async Task OnNewOrderToPayAsync_AddsMessage()
        {
            var message = new OrderToPayMessage();
            var service = CreateService();

            await service.OnNewOrderToPayAsync(message);
            _orderRepo.Verify(r => r.AddMessageAsync(message), Times.Once);
        }

        [Fact]
        public async Task TryReserveOrderToPayMessageAsync_ReservesAndReturnsMessage()
        {
            var id = Guid.NewGuid();
            var message = new OrderToPayMessage { OrderID = id };

            _orderRepo.Setup(r => r.TryReserveMessageAsync(It.IsAny<TimeSpan>())).ReturnsAsync(id);
            _orderRepo.Setup(r => r.GetMessageAsync(id)).ReturnsAsync(message);

            var service = CreateService();
            var result = await service.TryReserveOrderToPayMessageAsync(TimeSpan.FromMinutes(1));

            Assert.Equal(message, result);
        }

        [Fact]
        public async Task TryReserveOrderToPayMessageAsync_NoMessage_ReturnsNull()
        {
            _orderRepo.Setup(r => r.TryReserveMessageAsync(It.IsAny<TimeSpan>())).ReturnsAsync((Guid?)null);
            var service = CreateService();
            var result = await service.TryReserveOrderToPayMessageAsync(TimeSpan.FromMinutes(1));
            Assert.Null(result);
        }

        [Fact]
        public async Task PayForOrderAsync_AccountNotFound_SendsCanceledNoUserFound()
        {
            var message = new OrderToPayMessage { UserID = 1, Bill = 10, OrderID = Guid.NewGuid() };
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync((Account?)null);
            _orderRepo.Setup(r => r.DeleteMessageAsync(message.OrderID)).Returns(Task.CompletedTask);
            _paidRepo.Setup(r => r.AddMessageAsync(It.IsAny<PaidOrderMessage>())).Returns(Task.CompletedTask);

            var service = CreateService();
            await service.PayForOrderAsync(message);

            _paidRepo.Verify(p => p.AddMessageAsync(It.Is<PaidOrderMessage>(m =>
                JsonDocument.Parse(m.Payload, default)
                .RootElement.GetProperty("status")
                .GetString() == "CanceledNoUserFound")), Times.Once);
        }

        [Fact]
        public async Task PayForOrderAsync_InsufficientFunds_SendsCanceledNoFunds()
        {
            var message = new OrderToPayMessage { UserID = 1, Bill = 10, OrderID = Guid.NewGuid() };
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Account { Balance = 5 });

            var service = CreateService();
            await service.PayForOrderAsync(message);

            _paidRepo.Verify(p => p.AddMessageAsync(It.Is<PaidOrderMessage>(m =>
                JsonDocument.Parse(m.Payload, default)
                .RootElement.GetProperty("status")
                .GetString() == "CanceledNoFunds")), Times.Once);
        }

        [Fact]
        public async Task PayForOrderAsync_SufficientFunds_WithdrawsAndCompletes()
        {
            var message = new OrderToPayMessage { UserID = 1, Bill = 10, OrderID = Guid.NewGuid() };
            _accountRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Account { Balance = 100 });
            _accountRepository.Setup(r => r.ApplyOperationAsync(1, -10)).ReturnsAsync(true);

            var service = CreateService();
            await service.PayForOrderAsync(message);

            _paidRepo.Verify(p => p.AddMessageAsync(It.Is<PaidOrderMessage>(m =>
                JsonDocument.Parse(m.Payload, default)
                .RootElement.GetProperty("status")
                .GetString() == "Completed")), Times.Once);
        }

        [Fact]
        public async Task TryReservePaidOrderMessageAsync_ReservesMessage()
        {
            var id = Guid.NewGuid();
            var message = new PaidOrderMessage { OrderID = id };
            _paidRepo.Setup(r => r.TryReserveMessageAsync(It.IsAny<TimeSpan>())).ReturnsAsync(id);
            _paidRepo.Setup(r => r.GetMessageAsync(id)).ReturnsAsync(message);

            var service = CreateService();
            var result = await service.TryReservePaidOrderMessageAsync(TimeSpan.FromMinutes(1));

            Assert.Equal(message, result);
        }

        [Fact]
        public async Task DeletePaidOrderMessageAsync_Deletes()
        {
            var id = Guid.NewGuid();
            var service = CreateService();
            await service.DeletePaidOrderMessageAsync(id);
            _paidRepo.Verify(r => r.DeleteMessageAsync(id), Times.Once);
        }
    }
}
