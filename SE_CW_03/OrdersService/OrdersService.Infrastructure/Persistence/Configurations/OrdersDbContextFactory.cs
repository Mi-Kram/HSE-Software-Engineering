using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace OrdersService.Infrastructure.Persistence.Configurations
{
    // Для dotnet ef migrations
    public class OrdersDbContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
    {
        public OrdersDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<OrdersDbContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql("Host=localhost;Database=placeholder;Username=placeholder;Password=placeholder");
            return new OrdersDbContext(optionsBuilder.Options);
        }
    }
}
