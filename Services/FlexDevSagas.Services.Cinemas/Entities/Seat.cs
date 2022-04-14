using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlexDevSagas.Services.Cinemas.Entities
{
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }
        [JsonIgnore]
        [ForeignKey("Seat_Row")]
        public Row Row { get; set; }
    }
}
