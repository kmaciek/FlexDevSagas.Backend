using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Orders.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public List<Guid> Reservations { get; set; }
    }
}
