using FlexDevSagas.Services.Orders.Sagas.Order;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexDevSagas.Services.Orders.Context.SagaClassMaps
{
    public class OrderSagaStateMap : SagaClassMap<OrderSagaState>
    {
        protected override void Configure(EntityTypeBuilder<OrderSagaState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState);
        }
    }
}
