using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace PaymentsService.Infrastructure.Persistence.Configurations
{
    // Для dotnet ef migrations
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql("Host=localhost;Database=placeholder;Username=placeholder;Password=placeholder");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
