namespace FlexDevSagas.Common.Responses
{
    public record GetScheduledMovieDetailsResponse(
        Guid AuditoriumId,
        DateTime Start,
        DateTime End,
        int Price,
        string MovieName);
}
