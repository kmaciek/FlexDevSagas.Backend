using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Movies.Entities
{
    public class Movie
    {
        public Movie()
        {
            ScheduledMovies = new List<ScheduledMovie>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ScheduledMovie> ScheduledMovies { get; set; }
    }
}
