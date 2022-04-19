using FlexDevSagas.Common.Responses;
using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record OrderSagaStartedEvent(
        Guid CorrelationId,
        Guid OrderId,
        Guid ScheduledMovieId,
        IEnumerable<Guid> SeatIds) : CorrelatedBy<Guid>;
}
