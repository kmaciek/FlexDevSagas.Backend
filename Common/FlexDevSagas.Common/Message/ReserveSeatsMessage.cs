using MassTransit;

namespace FlexDevSagas.Common.Message
{
    public record ReserveSeatsMessage(
        Guid CorrelationId,
        Guid ScheduledMovieId,
        Guid OrderId,
        IEnumerable<Guid> ReservedSeats) : CorrelatedBy<Guid>;
}
    