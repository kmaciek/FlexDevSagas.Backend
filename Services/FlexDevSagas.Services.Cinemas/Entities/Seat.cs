using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexDevSagas.Services.Cinemas.Entities
{
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }
        [ForeignKey("Seat_Row")]
        public Row Row { get; set; }
    }
}
