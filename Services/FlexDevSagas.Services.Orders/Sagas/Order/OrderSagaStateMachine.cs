using FlexDevSagas.Common.Events;
using FlexDevSagas.Common.Message;
using FlexDevSagas.Services.Orders.Entities;
using MassTransit;

namespace FlexDevSagas.Services.Orders.Sagas.Order
{
    public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaState>
    {
        public State? OrderCreated { get; protected set; }
        public State? Reserved { get; protected set; }
        public State? Paid { get; protected set; }
        public State? Finished { get; protected set; }
        public State? Cancelled { get; protected set; }
        public State? Failed { get; protected set; }

        public Event<OrderSagaStartedEvent>? OrderSagaStarted { get; protected set; }
        public Event<SeatsReservedEvent>? SeatsReserved { get; protected set; }
        public Event<SeatsReservationRejectedEvent>? SeatsReservationRejected { get; protected set; }
        public Event<OrderPaidEvent>? OrderPaid { get; protected set; }
        public Event<TicketsCollectedEvent>? TicketsCollected { get; protected set; }
        public Event<OrderCancelledEvent>? OrderCancelled { get; protected set; }

        public OrderSagaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderSagaStarted);
            Event(() => SeatsReserved);
            Event(() => SeatsReservationRejected);
            Event(() => OrderPaid);
            Event(() => TicketsCollected);
            Event(() => OrderCancelled);

            Initially(
                When(OrderSagaStarted)
                    .Then(Initialize)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Created))
                    .ThenAsync(ReserveSeats)
                    .TransitionTo(OrderCreated));
            During(OrderCreated,
                When(SeatsReserved)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Reserved))
                    .ThenAsync(UpdateReservedSeats)
                    .TransitionTo(Reserved),
                When(SeatsReservationRejected)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Failed))
                    .TransitionTo(Failed));
            During(Reserved,
                When(OrderPaid)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Paid))
                    .TransitionTo(Paid),
                When(OrderCancelled)
                    .ThenAsync(ReleaseSeats));
            During(Paid,
                When(TicketsCollected)
                    .TransitionTo(Finished)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Finished))
                    .ThenAsync(CollectTickets),
                When(OrderCancelled)
                    .ThenAsync(ReleaseSeats)
                    .ThenAsync(ReturnPayment));
            DuringAny(
                When(OrderCancelled)
                    .ThenAsync((context) => UpdateOrderStatus(context, OrderState.Cancelled))
                    .TransitionTo(Cancelled));

            SetCompleted(async instance =>
            {
                var currentState = await this.GetState(instance);

                return Finished!.Equals(currentState) || Failed!.Equals(currentState) || Cancelled!.Equals(currentState);
            });
        }

        private async Task CollectTickets(BehaviorContext<OrderSagaState, TicketsCollectedEvent> context)
        {
            var message = new CollectedSeatsMessage(context.Message.ReservationIds);
            
            await context.Publish(message);
        }

        private async Task UpdateReservedSeats(BehaviorContext<OrderSagaState, SeatsReservedEvent> context)
        {
            var message = new OrderReservationChangedEvent(
                context.Saga.CorrelationId,
                context.Saga.OrderId,
                context.Message.ReservationIds);
            
            await context.Publish(message);
        }

        async Task ReturnPayment(BehaviorContext<OrderSagaState, OrderCancelledEvent> context)
        {
            var message = new ReturnPaymentMessage(context.Saga.CorrelationId);
            await context.Send(message);
        }

        async Task ReleaseSeats(BehaviorContext<OrderSagaState, OrderCancelledEvent> context)
        {
            var message = new ReleaseSeatsMessage(context.Saga.OrderId, context.Message.Reservations);
            await context.Send(message);
        }

        protected async Task ReserveSeats(BehaviorContext<OrderSagaState, OrderSagaStartedEvent> context)
        {
            var message = new ReserveSeatsMessage(context.Saga.CorrelationId,
                context.Message.ScheduledMovieId,
                context.Saga.OrderId,
                context.Message.SeatIds);

            await context.Send(message);
        }

        void Initialize(BehaviorContext<OrderSagaState, OrderSagaStartedEvent> context)
        {
            InitializeInstance(context.Saga, context.Message);
        }

        void InitializeInstance(OrderSagaState instance, OrderSagaStartedEvent data)
        {
            instance.CorrelationId = data.CorrelationId;
            instance.OrderId = data.OrderId;
        }

        async Task UpdateOrderStatus(BehaviorContext<OrderSagaState> context, OrderState orderState)
        {
            var @event = new OrderStatusChangedEvent(
                context.Saga.CorrelationId, 
                context.Saga.OrderId,
                (int)orderState);

            await context.Publish(@event);
        }
    }
}
