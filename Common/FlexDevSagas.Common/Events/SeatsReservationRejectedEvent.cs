using MassTransit;

namespace FlexDevSagas.Common.Events
{
    public record SeatsReservationRejectedEvent(Guid CorrelationId) : CorrelatedBy<Guid>;
}
