namespace FlexDevSagas.Common.Requests
{
    public record GetScheduledMoviesDetailsRequest(IEnumerable<Guid> ScheduledMoviesId);
}
