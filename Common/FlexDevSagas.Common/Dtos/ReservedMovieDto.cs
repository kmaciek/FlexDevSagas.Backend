namespace FlexDevSagas.Common.Dtos
{
    public record ReservedMovieDto(
        string Title,
        string Description,
        IEnumerable<ReservedSeatDto> ReservedSeats);
}
