using FlexDevSagas.Services.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Orders.Context
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .Property(x => x.Reservations)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse));

            modelBuilder
                .Entity<Order>()
                .Property(x => x.Seats)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Guid.Parse));
        }

        public DbSet<Order> Orders { get; set; }
    }
}
