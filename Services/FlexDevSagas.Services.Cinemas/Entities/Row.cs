using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("Row_Auditorium")]
        public Auditorium Auditoirum { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
