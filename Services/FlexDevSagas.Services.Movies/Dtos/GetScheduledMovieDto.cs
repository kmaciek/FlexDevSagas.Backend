namespace FlexDevSagas.Services.Movies.Dtos
{
    public record GetScheduledMovieDto(
        Guid Id,
        Guid AuditoriumId,
        Guid CinemaId,
        DateTime Start, 
        DateTime End,
        int Price,
        MovieDto movie);
}
