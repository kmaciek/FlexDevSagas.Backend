namespace FlexDevSagas.Common.Responses
{
    public record GetAuditoriumDetailsResponse(
        int Number,
        int Capacity,
        IEnumerable<Row> Rows);

    public record Row(
        int Number,
        IEnumerable<Seat> Seats);

    public class Seat
    {
        public Guid Id { get; init; }
        public int Number { get; init; }
        public bool IsReserved { get; set; }
    }
}
