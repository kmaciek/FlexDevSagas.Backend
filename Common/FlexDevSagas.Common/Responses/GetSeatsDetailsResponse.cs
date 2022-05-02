using FlexDevSagas.Common.Dtos;

namespace FlexDevSagas.Common.Responses
{
    public record GetSeatsDetailsResponse(
        IEnumerable<ReservedSeatDto> Seats);
}
