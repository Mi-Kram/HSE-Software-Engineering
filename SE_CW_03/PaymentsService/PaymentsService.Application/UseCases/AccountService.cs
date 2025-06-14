using Microsoft.Extensions.Logging;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Exceptions;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Domain.Interfaces.MessagesRepository;
using PaymentsService.Domain.Models;
using System.Text.Json;

namespace PaymentsService.Application.UseCases
{
    public class AccountService(
        IAccountRepository accountRepository,
        IOrderToPayMessagesRepository orderToPayMessagesRepository,
        IPaidOrderMessagesRepository paidOrderMessagesRepository,
        IDbTransaction dbTransaction,
        ILogger<AccountService> logger
        ) : IAccountService
    {
        private readonly IAccountRepository accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        private readonly IOrderToPayMessagesRepository orderToPayMessagesRepository = orderToPayMessagesRepository ?? throw new ArgumentNullException(nameof(orderToPayMessagesRepository));
        private readonly IPaidOrderMessagesRepository paidOrderMessagesRepository = paidOrderMessagesRepository ?? throw new ArgumentNullException(nameof(paidOrderMessagesRepository));
        private readonly IDbTransaction dbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
        private readonly ILogger<AccountService> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<List<Account>> GetAllAsync()
        {
            return await accountRepository.GetAllAsync();
        }

        public async Task<Account> GetAsync(int id)
        {
            return await accountRepository.GetAsync(id) ??
                throw new AccountNotExistsException(id); ;
        }

        public async Task<int> CreateAccountAsync(string caption, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(caption)) throw new ArgumentException("Название счета не указано");
            if (initialBalance < 0) throw new ArgumentException("Начальный баланс счета не может быть отрицательным");

            Account account = new()
            {
                Caption = caption.Trim(),
                Balance = initialBalance
            };

            return await accountRepository.CreateAccountAsync(account);
        }

        public async Task TopUpBalanceAsync(int id, decimal operation)
        {
            if (operation <= 0) throw new ArgumentException("Сумма пополнения счёта должна быть больше 0");

            Account _ = await accountRepository.GetAsync(id) ?? throw new AccountNotExistsException(id);
            
            if (!await accountRepository.ApplyOperationAsync(id, operation))
            {
                throw new Exception($"TopUpBalanceAsync: ошибка пополнения счета id = {id}, operation = {operation}");
            }
        }


        public async Task OnNewOrderToPayAsync(OrderToPayMessage message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            await orderToPayMessagesRepository.AddMessageAsync(message);
        }


        public async Task<OrderToPayMessage?> TryReserveOrderToPayMessageAsync(TimeSpan reserveTime)
        {
            Guid? orderID = await orderToPayMessagesRepository.TryReserveMessageAsync(reserveTime);
            if (orderID == null) return null;

            return await orderToPayMessagesRepository.GetMessageAsync(orderID.Value);
        }

        public async Task PayForOrderAsync(OrderToPayMessage message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await dbTransaction.BeginTransactionAsync();

            try
            {
                Account? account = await accountRepository.GetAsync(message.UserID);
                OrderStatus status;

                if (account == null) status = OrderStatus.CanceledNoUserFound;
                else if (account.Balance < message.Bill) status = OrderStatus.CanceledNoFunds;
                else
                {
                    if (!await accountRepository.ApplyOperationAsync(message.UserID, -message.Bill)) return;
                    status = OrderStatus.Completed;
                }

                string payload = JsonSerializer.Serialize(new
                {
                    order_id = message.OrderID,
                    status = status.ToString()
                });

                PaidOrderMessage paidMessage = new()
                {
                    OrderID = message.OrderID,
                    Payload = payload,
                    Reserved = DateTime.Now
                };

                await orderToPayMessagesRepository.DeleteMessageAsync(message.OrderID);
                await paidOrderMessagesRepository.AddMessageAsync(paidMessage);
                await dbTransaction.CommitTransactionAsync();
            }
            catch
            {
                await dbTransaction.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<PaidOrderMessage?> TryReservePaidOrderMessageAsync(TimeSpan reserveTime)
        {
            Guid? orderID = await paidOrderMessagesRepository.TryReserveMessageAsync(reserveTime);
            if (orderID == null) return null;

            return await paidOrderMessagesRepository.GetMessageAsync(orderID.Value);
        }

        public async Task DeletePaidOrderMessageAsync(Guid orderID)
        {
            await paidOrderMessagesRepository.DeleteMessageAsync(orderID);
        }
    }
}
