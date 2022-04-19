namespace FlexDevSagas.Services.Orders.Dtos
{
    public record GetOrdersListDto(
        IEnumerable<OrderListItemDto> Orders);
}
