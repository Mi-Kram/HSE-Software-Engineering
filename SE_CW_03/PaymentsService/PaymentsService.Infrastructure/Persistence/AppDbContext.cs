using Microsoft.EntityFrameworkCore;
using PaymentsService.Domain.Entities;
using PaymentsService.Infrastructure.DTO;
using PaymentsService.Infrastructure.Persistence.Configurations;

namespace PaymentsService.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<OrderToPayMessage> OrderToPayMessages { get; set; }
        public virtual DbSet<PaidOrderMessage> PaidOrderMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new OrderToPayMessageConfiguration());
            modelBuilder.ApplyConfiguration(new PaidOrderMessageConfiguration());
        }
    }
}
