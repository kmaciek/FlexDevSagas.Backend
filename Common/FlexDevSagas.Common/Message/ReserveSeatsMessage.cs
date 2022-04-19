using MassTransit;

namespace FlexDevSagas.Common.Message
{
    public record ReserveSeatsMessage(
        Guid CorrelationId,
        Guid ScheduledMovieId,
        IEnumerable<Guid> ReservedSeats) : CorrelatedBy<Guid>;
}
    