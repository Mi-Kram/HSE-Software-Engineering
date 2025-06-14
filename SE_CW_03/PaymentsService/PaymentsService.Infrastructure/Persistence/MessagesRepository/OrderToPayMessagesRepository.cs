using Microsoft.EntityFrameworkCore;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces.MessagesRepository;
using PaymentsService.Infrastructure.DTO;

namespace PaymentsService.Infrastructure.Persistence.MessagesRepository
{
    public class OrderToPayMessagesRepository(AppDbContext context) : IOrderToPayMessagesRepository
    {
        private readonly AppDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<OrderToPayMessage?> GetMessageAsync(Guid orderID)
        {
            return await context.OrderToPayMessages.AsNoTracking()
                .SingleOrDefaultAsync(x => x.OrderID == orderID);
        }

        public async Task AddMessageAsync(OrderToPayMessage message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await context.OrderToPayMessages.AddAsync(message);
            await context.SaveChangesAsync();
        }

        public async Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime)
        {
            if (reserveTime.TotalSeconds <= 0) throw new ArgumentException("Время для резервирования сообщения должно быть больше 0");

            OrderIdDTO? orderID = await context.Database.SqlQueryRaw<OrderIdDTO>(
                @"SELECT * FROM try_reserve_order_to_pay_message({0})", (int)reserveTime.TotalSeconds)
                .FirstOrDefaultAsync();

            return orderID?.OrderID;
        }

        public async Task DeleteMessageAsync(Guid orderID)
        {
            await context.OrderToPayMessages
                .Where(x => x.OrderID == orderID)
                .ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }
    }
}
