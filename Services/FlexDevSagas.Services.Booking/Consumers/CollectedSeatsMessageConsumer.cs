using FlexDevSagas.Common.Message;
using FlexDevSagas.Services.Booking.Context;
using FlexDevSagas.Services.Booking.Entities;
using MassTransit;

namespace FlexDevSagas.Services.Booking.Consumers
{
    public class CollectedSeatsMessageConsumer : IConsumer<CollectedSeatsMessage>
    {
        private readonly BookingContext _dbContext;

        public CollectedSeatsMessageConsumer(BookingContext context)
        {
            _dbContext = context;
        }
        
        public async Task Consume(ConsumeContext<CollectedSeatsMessage> context)
        {
            var reservations = _dbContext.Reservations.Where(x => context.Message.ReservationIds.Contains(x.Id));
            foreach (var reservation in reservations)
            {
                reservation.Status = ReservationStatus.Collected;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
