using FlexDevSagas.Common.Events;
using FlexDevSagas.Services.Orders.Context;
using FlexDevSagas.Services.Orders.Entities;
using MassTransit;

namespace FlexDevSagas.Services.Orders.Consumers
{
    public class OrderStatusChangedEventConsumer : IConsumer<OrderStatusChangedEvent>
    {
        private readonly OrdersContext _dbContext;

        public OrderStatusChangedEventConsumer(OrdersContext context)
        {
            _dbContext = context;
        }
        
        public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == context.Message.OrderId);

            if (order == null)
            {
                return;
            }
            
            order.OrderState = (OrderState)context.Message.Status;
            await _dbContext.SaveChangesAsync();
        }
    }
}
