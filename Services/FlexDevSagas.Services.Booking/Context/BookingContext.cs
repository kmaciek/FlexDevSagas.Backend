using FlexDevSagas.Services.Booking.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Booking.Context
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.Id)
                .IsUnique();
        }
    }
}
