using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlexDevSagas.Services.Movies.Entities
{
    public class ScheduledMovie
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Price { get; set; }
        public Movie Movie { get; set; }

        public Guid AuditoriumId { get; set; }
        public Guid CinemaId { get; set; }
    }
}
