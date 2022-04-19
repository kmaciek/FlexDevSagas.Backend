using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record TicketsCollectedEvent(Guid CorrelationId) : CorrelatedBy<Guid>;
}
