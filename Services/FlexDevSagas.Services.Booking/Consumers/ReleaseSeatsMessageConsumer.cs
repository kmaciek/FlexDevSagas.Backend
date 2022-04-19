using FlexDevSagas.Common.Message;
using FlexDevSagas.Services.Booking.Context;
using FlexDevSagas.Services.Booking.Entities;
using MassTransit;

namespace FlexDevSagas.Services.Booking.Consumers
{
    public class ReleaseSeatsMessageConsumer : IConsumer<ReleaseSeatsMessage>
    {
        private readonly BookingContext _dbContext;

        public ReleaseSeatsMessageConsumer(BookingContext context)
        {
            _dbContext = context;
        }

        public async Task Consume(ConsumeContext<ReleaseSeatsMessage> context)
        {
            var reservations = _dbContext.Reservations
                .Where(r => context.Message.ReservationIds.Contains(r.Id))
                .ToList();

            foreach (var reservation in reservations)
            {
                reservation.Status = ReservationStatus.Released;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
