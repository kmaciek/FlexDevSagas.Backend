namespace FlexDevSagas.Services.Orders.Dtos
{
    public record CreateOrderDto(
        Guid ScheduledMovieId,
        IEnumerable<Guid> Seats);
}
