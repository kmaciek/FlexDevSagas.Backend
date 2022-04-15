using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Cinemas.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Cinemas.Consumers
{
    public class GetAuditoriumDetailsRequestConsumer : IConsumer<GetAuditoriumDetailsRequest>
    {
        private readonly CinemaContext _context;
        
        public GetAuditoriumDetailsRequestConsumer(CinemaContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<GetAuditoriumDetailsRequest> context)
        {
            var auditorium = _context.Auditoriums
                .Include(a => a.Rows)
                .ThenInclude(r => r.Seats)
                .FirstOrDefault(a => a.Id == context.Message.AuditoriumId);

            if (auditorium == null)
            {
                throw new ArgumentException();
            }
            
            await context.RespondAsync(new GetAuditoriumDetailsResponse(
                auditorium.Number,
                auditorium.Capacity,
                auditorium.Rows.Select(r => new Row(
                    r.Number,
                    r.Seats.Select(s => new Seat
                    {
                        Id = s.Id,
                        Number = s.Number,
                        IsReserved = false
                    })))));
        }
    }
}
