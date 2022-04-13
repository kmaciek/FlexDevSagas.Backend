using System.ComponentModel.DataAnnotations.Schema;

namespace FlexDevSagas.Services.Cinemas.Entities
{
    public class Auditorium
    {
        public Auditorium()
        {
            Rows = new List<Row>();
        }

        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public List<Row> Rows { get; set; }
        [ForeignKey("Auditorium_Cinema")]
        public Cinema Cinema { get; set; }
    }
}
