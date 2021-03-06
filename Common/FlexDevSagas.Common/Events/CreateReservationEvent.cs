using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record CreateReservationEvent(
        Guid CorrelationId,
        Guid ScheduledMovieId,
        IEnumerable<Guid> SeatIds) : CorrelatedBy<Guid>;
}
