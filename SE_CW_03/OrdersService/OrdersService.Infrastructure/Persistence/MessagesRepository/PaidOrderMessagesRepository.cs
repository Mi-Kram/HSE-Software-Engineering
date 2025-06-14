using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces.MessagesRepository;
using OrdersService.Infrastructure.DTO;

namespace OrdersService.Infrastructure.Persistence.MessagesRepository
{
    public class PaidOrderMessagesRepository(OrdersDbContext context) : IPaidOrderMessagesRepository
    {
        private readonly OrdersDbContext context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<PaidOrderMessage?> GetMessageAsync(Guid orderID)
        {
            return await context.PaidOrderMessages.AsNoTracking()
                .SingleOrDefaultAsync(x => x.OrderID == orderID);
        }

        public async Task AddMessageAsync(PaidOrderMessage message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            await context.PaidOrderMessages.AddAsync(message);
            await context.SaveChangesAsync();
        }

        public async Task<Guid?> TryReserveMessageAsync(TimeSpan reserveTime)
        {
            if (reserveTime.TotalSeconds <= 0) throw new ArgumentException("Время для резервирования сообщения должно быть больше 0");

            OrderIdDTO? orderID = await context.Database.SqlQueryRaw<OrderIdDTO>(
                @"SELECT * FROM try_reserve_paid_order_message({0})", (int)reserveTime.TotalSeconds)
                .FirstOrDefaultAsync();

            return orderID?.OrderID;
        }

        public async Task DeleteMessageAsync(Guid orderID)
        {
            await context.PaidOrderMessages
                .Where(x => x.OrderID == orderID)
                .ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }
    }
}
