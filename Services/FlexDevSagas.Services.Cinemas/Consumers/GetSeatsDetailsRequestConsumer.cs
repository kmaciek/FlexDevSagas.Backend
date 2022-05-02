using FlexDevSagas.Common.Dtos;
using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Cinemas.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Cinemas.Consumers
{
    public class GetSeatsDetailsRequestConsumer : IConsumer<GetSeatsDetailsRequest>
    {
        private readonly CinemaContext _dbContext;

        public GetSeatsDetailsRequestConsumer(CinemaContext context)
        {
            _dbContext = context;
        }
        
        public async Task Consume(ConsumeContext<GetSeatsDetailsRequest> context)
        {
            var seats = _dbContext.Seats
                .Include(x => x.Row)
                .Where(x => context.Message.Seats.Contains(x.Id))
                .ToList();
            
            var response = new GetSeatsDetailsResponse(seats.Select(x => new ReservedSeatDto(x.Id, x.Row.Number, x.Number)));
            await context.RespondAsync(response);
        }
    }
}
