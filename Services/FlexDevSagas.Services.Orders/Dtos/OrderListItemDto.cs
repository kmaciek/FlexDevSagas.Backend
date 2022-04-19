namespace FlexDevSagas.Services.Orders.Dtos
{
    public record OrderListItemDto(
        Guid Id,
        int NumberOfSeats,
        int TotalPrice,
        string OrderState
        );
}
