using FlexDevSagas.Services.Cinemas.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Cinemas.Context
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options) { }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Seat> Seats { get; set; }
    }
}
