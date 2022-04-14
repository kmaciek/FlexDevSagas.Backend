using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlexDevSagas.Services.Cinemas.Entities
{
    public class Row
    {
        public Row()
        {
            Seats = new List<Seat>();
        }

        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }

        [JsonIgnore]
        [ForeignKey("Row_Auditorium")]
        public Auditorium Auditoirum { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
