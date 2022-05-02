using FlexDevSagas.Common.Dtos;

namespace FlexDevSagas.Common.Responses
{
    public record GetReservationDetailsResponse(
        IEnumerable<ReservedMovieDto> Movies);
}
