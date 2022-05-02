using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Orders.Entities
{
    public class Order
    {
        public Order()
        {
            Reservations = new List<Guid>();
            Seats = new List<Guid>();
        }

        [Key]
        public Guid Id { get; set; }
        public IEnumerable<Guid> Reservations { get; set; }

        public IEnumerable<Guid> Seats { get; set; }
        public Guid ScheduledMovieId { get; set; }
        public int TotalPrice { get; set; }
        public OrderState OrderState { get; set; }
    }
}
