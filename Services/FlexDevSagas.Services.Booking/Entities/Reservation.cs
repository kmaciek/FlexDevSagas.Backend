using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Booking.Entities
{
    
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid SeatId { get; set; }
        public Guid MovieId { get; set; }
        public Guid OrderId { get; set; }
        public bool Paid { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
