using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record OrderCancelledEvent(
        Guid CorrelationId,
        IEnumerable<Guid> Reservations) : CorrelatedBy<Guid>;
}
