using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Entities;
using OrdersService.Infrastructure.Persistence.Configurations;

namespace OrdersService.Infrastructure.Persistence
{
    public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<NewOrderMessage> NewOrderMessages { get; set; }
        public virtual DbSet<PaidOrderMessage> PaidOrderMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrdersConfiguration());
            modelBuilder.ApplyConfiguration(new NewOrderMessageConfiguration());
            modelBuilder.ApplyConfiguration(new PaidOrderMessageConfiguration());
        }
    }
}
