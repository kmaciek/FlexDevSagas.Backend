namespace FlexDevSagas.Services.Orders.Dtos
{
    public record OrderDetailsDto(
        Guid Id,
        IEnumerable<Guid> Reservations,
        Guid ScheduledMovieId,
        int TotalPrice,
        string OrderStatus);
}
