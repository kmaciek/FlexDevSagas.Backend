using MassTransit;

namespace FlexDevSagas.Services.Orders.Sagas.Order
{
    public class OrderSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        
        public int CurrentState { get; set; }
        public Guid OrderId { get; set; }
    }
}
