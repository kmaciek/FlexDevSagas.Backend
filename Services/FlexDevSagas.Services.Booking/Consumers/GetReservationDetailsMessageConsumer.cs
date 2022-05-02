using FlexDevSagas.Common.Dtos;
using FlexDevSagas.Common.Message;
using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Booking.Context;
using MassTransit;

namespace FlexDevSagas.Services.Booking.Consumers
{
    public class GetReservationDetailsMessageConsumer : IConsumer<GetReservationDetailsMessage>
    {
        private readonly BookingContext _dbContext;
        private readonly IRequestClient<GetScheduledMoviesDetailsRequest> _scheduledMoviesDetailsRequestClient;
        private readonly IRequestClient<GetSeatsDetailsRequest> _seatsDetailsRequestClient;

        public GetReservationDetailsMessageConsumer(
            BookingContext context,
            IRequestClient<GetScheduledMoviesDetailsRequest> scheduledMoviesDetailsRequestClient,
            IRequestClient<GetSeatsDetailsRequest> seatsDetailsRequestClient)
        {
            _dbContext = context;
            _scheduledMoviesDetailsRequestClient = scheduledMoviesDetailsRequestClient;
            _seatsDetailsRequestClient = seatsDetailsRequestClient;
        }
        
        public async Task Consume(ConsumeContext<GetReservationDetailsMessage> context)
        {
            var reservations = _dbContext.Reservations.Where(x => x.OrderId == context.Message.OrderId).ToList();

            if (!reservations.Any())
            {
                await context.RespondAsync(new GetReservationDetailsResponse(new List<ReservedMovieDto>()));
                return;
            }

            var movies = reservations.Select(x => x.MovieId).Distinct().ToList();
            var seats = reservations.Select(x => x.SeatId).Distinct().ToList();

            var moviesRequest = new GetScheduledMoviesDetailsRequest(movies);
            var moviesDetails =
                await _scheduledMoviesDetailsRequestClient.GetResponse<GetScheduledMoviesDetailsResponse>(moviesRequest);

            var seatsDetailsRequest = new GetSeatsDetailsRequest(seats);
            var seatsDetails = _seatsDetailsRequestClient.GetResponse<GetSeatsDetailsResponse>(seatsDetailsRequest);

            var responseMovies = new List<ReservedMovieDto>();
            foreach (var movie in moviesDetails.Message.Movies)
            {
                var seatsToSelect = reservations.Where(x => x.MovieId == movie.Id).Select(x => x.SeatId);
                var movieDto = new ReservedMovieDto(
                    movie.MovieName,
                    string.Empty,
                    seatsDetails.Result.Message.Seats.Where(x => seatsToSelect.Contains(x.Id)));
                responseMovies.Add(movieDto);
            }
            
            await context.RespondAsync(new GetReservationDetailsResponse(responseMovies));
        }
    }
}
