namespace FlexDevSagas.Common.Events
{
    public record OrderReservationChangedEvent(
        Guid CorrelationId,
        Guid OrderId,
        IEnumerable<Guid> ReservationIds);
}
