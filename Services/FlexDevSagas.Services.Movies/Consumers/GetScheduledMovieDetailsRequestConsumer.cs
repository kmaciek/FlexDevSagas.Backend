using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Movies.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Movies.Consumers
{
    public class GetScheduledMovieDetailsRequestConsumer : IConsumer<GetScheduledMovieDetailsRequest>
    {
        private readonly MovieContext _context;

        public GetScheduledMovieDetailsRequestConsumer(MovieContext context)
        {
            _context = context;
        }
        
        public async Task Consume(ConsumeContext<GetScheduledMovieDetailsRequest> context)
        {
            var movie = _context.ScheduledMovies
                .Include(sm => sm.Movie)
                .FirstOrDefault(sm => sm.Id == context.Message.ScheduledMovieId);

            if (movie == null)
            {
                throw new ArgumentException();
            }

            await context.RespondAsync(new GetScheduledMovieDetailsResponse(
                movie.AuditoriumId,
                movie.Start,
                movie.End,
                movie.Price,
                movie.Movie.Name
            ));
        }
    }
}
