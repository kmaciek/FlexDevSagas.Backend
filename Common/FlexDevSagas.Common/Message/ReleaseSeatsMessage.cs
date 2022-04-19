using MassTransit;

namespace FlexDevSagas.Common.Message
{
    public record ReleaseSeatsMessage(
        Guid CorrelationId,
        IEnumerable<Guid> ReservationIds) : CorrelatedBy<Guid>;
}
