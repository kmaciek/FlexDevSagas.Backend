using FlexDevSagas.Common.Dtos;

namespace FlexDevSagas.Services.Orders.Dtos
{
    public record OrderDetailsDto(
        Guid Id,
        IEnumerable<ReservedMovieDto> Movies,
        int TotalPrice,
        string OrderStatus);
}
