using FlexDevSagas.Services.Movies.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Movies.Context
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<ScheduledMovie> ScheduledMovies { get; set; }
    }
}
