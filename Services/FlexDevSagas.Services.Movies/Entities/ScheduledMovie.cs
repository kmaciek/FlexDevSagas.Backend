using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexDevSagas.Services.Movies.Entities
{
    public class ScheduledMovie
    {
        [Key]
        public string Id { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Price { get; set; }
        
        [ForeignKey("ScheduledMovie_Movie")]
        public Movie Movie { get; set; }

        public Guid AuditoriumId { get; set; }
    }
}
