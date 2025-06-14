using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.DTO;
using OrdersService.Domain.Entities;
using OrdersService.Domain.Interfaces;

namespace OrdersService.Infrastructure.Persistence
{
    public class OrdersRepository(OrdersDbContext context) : IOrdersRepository
    {
        private readonly OrdersDbContext context = context ?? throw new ArgumentNullException(nameof(context));
        
        public async Task<List<Order>> GetAllAsync()
        {
            return await context.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<List<Order>> GetAllByUserIDAsync(int userID)
        {
            return await context.Orders.AsNoTracking()
                .Where(x => x.UserID == userID)
                .ToListAsync();
        }

        public async Task<Order?> GetAsync(Guid orderID)
        {
            return await context.Orders.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ID == orderID);
        }

        public async Task<Order> AddOrderAsync(OrderDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            Order order = new()
            {
                ID = Guid.NewGuid(),
                UserID = dto.UserID,
                Bill = dto.Bill,
                CreatedAt = dto.CreatedAt,
                Status = OrderStatus.NewOrder
            };

            await context.AddAsync(order);
            await context.SaveChangesAsync();

            return order;
        }

        public async Task SetStatusAsync(Guid orderID, OrderStatus status)
        {
            await context.Orders
                .Where(x => x.ID == orderID)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Status, status));
            await context.SaveChangesAsync();
        }
    }
}
