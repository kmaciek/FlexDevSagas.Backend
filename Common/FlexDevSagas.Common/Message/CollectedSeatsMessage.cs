namespace FlexDevSagas.Common.Message
{
    public record CollectedSeatsMessage(IEnumerable<Guid> ReservationIds);
}
