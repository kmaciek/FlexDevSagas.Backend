using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record OrderStatusChangedEvent(
        Guid CorrelationId,
        Guid OrderId,
        int Status) : CorrelatedBy<Guid>;
}
