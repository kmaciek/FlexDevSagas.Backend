namespace FlexDevSagas.Services.Movies.Dtos
{
    public record PostScheduledMovieDto(
        Guid MovieId,
        Guid AuditoriumId,
        Guid CinemaId,
        DateTime Start, 
        DateTime End,
        int Price);
}
