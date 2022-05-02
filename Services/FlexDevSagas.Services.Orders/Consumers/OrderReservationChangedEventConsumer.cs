using FlexDevSagas.Common.Events;
using FlexDevSagas.Services.Orders.Context;
using MassTransit;

namespace FlexDevSagas.Services.Orders.Consumers
{
    public class OrderReservationChangedEventConsumer : IConsumer<OrderReservationChangedEvent>
    {
        private readonly OrdersContext _dbContext;

        public OrderReservationChangedEventConsumer(OrdersContext context)
        {
            _dbContext = context;
        }
        
        public async Task Consume(ConsumeContext<OrderReservationChangedEvent> context)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == context.Message.OrderId);

            if (order == null)
            {
                return;
            }

            order.Reservations = context.Message.ReservationIds;
            await _dbContext.SaveChangesAsync();
        }
    }
}
