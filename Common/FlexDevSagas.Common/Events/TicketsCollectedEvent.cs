using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record TicketsCollectedEvent(Guid CorrelationId, IEnumerable<Guid> ReservationIds) : CorrelatedBy<Guid>;
}
