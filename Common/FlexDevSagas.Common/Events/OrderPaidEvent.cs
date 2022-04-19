using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record OrderPaidEvent(
        Guid CorrelationId) : CorrelatedBy<Guid>;
}
