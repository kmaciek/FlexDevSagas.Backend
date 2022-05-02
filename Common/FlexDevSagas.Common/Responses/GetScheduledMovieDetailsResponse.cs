namespace FlexDevSagas.Common.Responses
{
    public record GetScheduledMovieDetailsResponse(
        Guid Id,
        Guid AuditoriumId,
        DateTime Start,
        DateTime End,
        int Price,
        string MovieName);
}
