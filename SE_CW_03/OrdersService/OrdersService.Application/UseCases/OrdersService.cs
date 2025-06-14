using Microsoft.Extensions.Logging;
using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Exceptions;
using OrdersService.Domain.Interfaces;
using OrdersService.Domain.Interfaces.MessagesRepository;
using System.Text.Json;

namespace OrdersService.Application.UseCases
{
    public class OrdersService(
        IOrdersRepository orderRepository,
        INewOrderMessagesRepository newOrderMessagesRepository,
        IPaidOrderMessagesRepository paidOrderMessagesRepository,
        IDbTransaction dbTransaction,
        ILogger<OrdersService> logger
        ) : IOrdersService
    {
        private readonly IOrdersRepository orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly INewOrderMessagesRepository newOrderMessagesRepository = newOrderMessagesRepository ?? throw new ArgumentNullException(nameof(newOrderMessagesRepository));
        private readonly IPaidOrderMessagesRepository paidOrderMessagesRepository = paidOrderMessagesRepository ?? throw new ArgumentNullException(nameof(paidOrderMessagesRepository));
        private readonly IDbTransaction dbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
        private readonly ILogger<OrdersService> logger = logger ?? throw new ArgumentNullException(nameof(logger));


        public async Task<List<Order>> GetAllAsync()
        {
            return await orderRepository.GetAllAsync();
        }

        public async Task<List<Order>> GetAllByUserIDAsync(int userID)
        {
            return await orderRepository.GetAllByUserIDAsync(userID);
        }

        public async Task<Order> GetAsync(Guid orderID)
        {
            return await orderRepository.GetAsync(orderID)
                ?? throw new OrderNotExistsException(orderID);
        }

        public async Task<Guid> CreateOrderAsync(OrderDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            if (dto.Bill <= 0) throw new ArgumentException("Счет заказа должен быть положительным");

            await dbTransaction.BeginTransactionAsync();

            try
            {
                Order order = await orderRepository.AddOrderAsync(dto);

                string payload = JsonSerializer.Serialize(new
                {
                    order_id = order.ID,
                    user_id = order.UserID,
                    created_at = order.CreatedAt,
                    bill = order.Bill
                });

                NewOrderMessage message = new()
                {
                    OrderID = order.ID,
                    UserID = order.UserID,
                    CreatedAt = order.CreatedAt,
                    Reserved = DateTime.Now,
                    Payload = payload
                };

                await newOrderMessagesRepository.AddMessageAsync(message);
                await dbTransaction.CommitTransactionAsync();

                return order.ID;
            }
            catch
            {
                await dbTransaction.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<NewOrderMessage?> TryReserveNewOrderMessageAsync(TimeSpan reserveTime)
        {
            Guid? orderID = await newOrderMessagesRepository.TryReserveMessageAsync(reserveTime);
            if (orderID == null) return null;
            return await newOrderMessagesRepository.GetMessageAsync(orderID.Value);
        }

        public async Task AwaitForPaymentAsync(Guid orderID)
        {
            await orderRepository.SetStatusAsync(orderID, OrderStatus.AwaitForPayment);
            await newOrderMessagesRepository.DeleteMessageAsync(orderID);
        }

        public async Task OnPaymentResultReceivedAsync(Guid orderID, OrderStatus status)
        {
            PaidOrderMessage message = new()
            {
                OrderID = orderID,
                Status = status,
                Reserved = DateTime.Now
            };

            await paidOrderMessagesRepository.AddMessageAsync(message);
        }

        public async Task<PaidOrderMessage?> TryReservePaidOrderMessageAsync(TimeSpan reserveTime)
        {
            Guid? orderID = await paidOrderMessagesRepository.TryReserveMessageAsync(reserveTime);
            if (orderID == null) return null;
            return await paidOrderMessagesRepository.GetMessageAsync(orderID.Value);
        }

        public async Task CompleteOrderAsync(PaidOrderMessage message)
        {
            switch (message.Status)
            {
                case OrderStatus.Completed:
                case OrderStatus.CanceledNoUserFound:
                case OrderStatus.CanceledNoFunds:
                    break;
                default:
                    logger.LogError("CompleteOrderAsync: неверный статус {status}", message.Status.ToString());
                    return;
            }

            await orderRepository.SetStatusAsync(message.OrderID, message.Status);
            await paidOrderMessagesRepository.DeleteMessageAsync(message.OrderID);
        }
    }
}
