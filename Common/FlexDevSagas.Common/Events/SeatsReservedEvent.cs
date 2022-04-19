using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record SeatsReservedEvent(
        Guid CorrelationId,
        IEnumerable<Guid> ReservationIds) : CorrelatedBy<Guid>;
}
