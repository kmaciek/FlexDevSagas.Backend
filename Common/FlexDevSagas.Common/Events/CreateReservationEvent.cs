using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record CreateReservationEvent(Guid CorrelationId) : CorrelatedBy<Guid>;
}
