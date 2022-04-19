using FlexDevSagas.Common.Events;
using FlexDevSagas.Common.Message;
using FlexDevSagas.Services.Booking.Context;
using FlexDevSagas.Services.Booking.Entities;
using MassTransit;

namespace FlexDevSagas.Services.Booking.Consumers
{
    public class ReserveSeatsMessageConsumer : IConsumer<ReserveSeatsMessage>
    {
        private readonly BookingContext _dbContext;

        public ReserveSeatsMessageConsumer(BookingContext context)
        {
            _dbContext = context;
        }
        
        public async Task Consume(ConsumeContext<ReserveSeatsMessage> context)
        {
            if (context.Message.ReservedSeats.Count() > 5)
            {
                await context.Publish(new SeatsReservationRejectedEvent(
                    context.Message.CorrelationId));
                return;
            }
            
            var reservations = new List<Reservation>();

            foreach (var seat in context.Message.ReservedSeats)
            {
                var reservation = new Reservation()
                {
                    SeatId = seat,
                    MovieId = context.Message.ScheduledMovieId,
                    Status = ReservationStatus.Reserved
                };

                reservations.Add(reservation);
            }

            _dbContext.Reservations.AddRange(reservations);
            await _dbContext.SaveChangesAsync();
            
            await context.Publish(new SeatsReservedEvent(
                context.Message.CorrelationId,
                reservations.Select(x => x.Id)));
        }
    }
}
