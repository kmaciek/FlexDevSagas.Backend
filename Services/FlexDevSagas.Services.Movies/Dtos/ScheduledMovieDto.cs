namespace FlexDevSagas.Services.Movies.Dtos
{
    public record ScheduledMovieDto(
        Guid MovieId,
        Guid AuditoriumId, 
        DateTime Start, 
        DateTime End,
        int Price);
}
