using FlexDevSagas.Services.Orders.Context.SagaClassMaps;
using FlexDevSagas.Services.Orders.Sagas.Order;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace FlexDevSagas.Services.Orders.Context
{
    public class OrderSagaDbContext : SagaDbContext
    {
        public OrderSagaDbContext(DbContextOptions<OrderSagaDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderSagaStateMap(); }
        }

        public DbSet<OrderSagaState> OrderSagaStates { get; set; } = null!;
    }
}
