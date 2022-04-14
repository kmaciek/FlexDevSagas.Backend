using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Cinema Cinema { get; set; }
    }
}
