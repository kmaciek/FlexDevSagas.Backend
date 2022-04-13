using System.ComponentModel.DataAnnotations;

namespace FlexDevSagas.Services.Cinemas.Entities
{
    public class Cinema
    {
        public Cinema()
        {
            Auditoriums = new List<Auditorium>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Auditorium> Auditoriums { get; set; }
    }
}
