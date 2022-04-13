using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Booking.Entities
{
    
    public class Reservation
    {
        public Guid Id { get; set; }

        [Key]
        public Guid SeatId { get; set; }
        [Key]
        public Guid MovieId { get; set; }
    }
}
