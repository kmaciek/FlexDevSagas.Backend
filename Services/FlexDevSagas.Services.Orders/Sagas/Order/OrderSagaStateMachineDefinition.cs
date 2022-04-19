using MassTransit;

namespace FlexDevSagas.Services.Orders.Sagas.Order
{
    public class OrderSagaStateMachineDefinition : SagaDefinition<OrderSagaState>
    {
        public OrderSagaStateMachineDefinition()
        {
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(
            IReceiveEndpointConfigurator endpointConfigurator, 
            ISagaConfigurator<OrderSagaState> sagaConfigurator)
        {
            sagaConfigurator.UseMessageRetry(r => r.Immediate(5));
        }
    }
}
