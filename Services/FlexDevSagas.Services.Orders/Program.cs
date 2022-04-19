using System.Reflection;
using FlexDevSagas.Common.Config;
using FlexDevSagas.Common.Events;
using FlexDevSagas.Common.Message;
using FlexDevSagas.Common.Providers;
using FlexDevSagas.Services.Orders.Consumers;
using FlexDevSagas.Services.Orders.Context;
using FlexDevSagas.Services.Orders.Dtos;
using FlexDevSagas.Services.Orders.Entities;
using FlexDevSagas.Services.Orders.Sagas.Order;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<OrdersContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<OrderSagaDbContext>(options =>
    options.UseSqlServer(connectionString));

var rabbitMqConfig = new RabbitMQConfig();
builder.Configuration.Bind("RabbitMQConfig", rabbitMqConfig);
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
    cfg.AddSagaStateMachine<OrderSagaStateMachine, OrderSagaState>(typeof(OrderSagaStateMachineDefinition))
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.LockStatementProvider = new CustomSqlLockStatementProvider("OrderSagaStates");

            r.AddDbContext<DbContext, OrderSagaDbContext>((_, dbBuilder) =>
            {
                dbBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Database"), m =>
                {
                    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    m.MigrationsHistoryTable($"__{nameof(OrderSagaDbContext)}");
                });
            });
        });

    cfg.UsingRabbitMq((x, y) =>
    {
        y.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h =>
        {
            h.Username(rabbitMqConfig.Username);
            h.Password(rabbitMqConfig.Password);
        });

        var endpointNameFormatter = x.GetRequiredService<IEndpointNameFormatter>();
        EndpointConvention.Map<ReserveSeatsMessage>(new Uri($"queue:{endpointNameFormatter.Message<ReserveSeatsMessage>()}"));
        EndpointConvention.Map<ReleaseSeatsMessage>(new Uri($"queue:{endpointNameFormatter.Message<ReleaseSeatsMessage>()}"));
        EndpointConvention.Map<OrderSagaStartedEvent>(new Uri($"queue:{endpointNameFormatter.Message<OrderSagaStartedEvent>()}"));
        EndpointConvention.Map<SeatsReservedEvent>(new Uri($"queue:{endpointNameFormatter.Message<SeatsReservedEvent>()}"));
        EndpointConvention.Map<OrderStatusChangedEvent>(new Uri($"queue:{endpointNameFormatter.Message<SeatsReservedEvent>()}"));
        
        y.ConfigureEndpoints(x);
    });
});
 
var app = builder.Build();
app.UseCors("AllowAll");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrdersContext>();
    context.Database.Migrate();

    var sagaContext = services.GetRequiredService<OrderSagaDbContext>();
    sagaContext.Database.Migrate();
}

app.MapPost("/", (CreateOrderDto dto, OrdersContext dbContext, IBus bus) =>
{
    var order = new Order()
    {
        Reservations = new List<Guid>(),
        OrderState = OrderState.Created,
        ScheduledMovieId = dto.ScheduledMovieId,
        Seats = dto.Seats,
        TotalPrice = 100
    };

    dbContext.Orders.Add(order);
    dbContext.SaveChangesAsync();

    bus.Publish(new OrderSagaStartedEvent(Guid.NewGuid(), order.Id, order.ScheduledMovieId, order.Seats));
});

app.MapGet("/", (OrdersContext dbContext) =>
{
    var orders = dbContext.Orders.Select(
        o => new OrderListItemDto(
            o.Id,
            o.Seats.Count(),
            o.TotalPrice,
            o.OrderState.ToString())).ToList();
    
    return new GetOrdersListDto(orders);
});

app.MapGet("/{id:guid}", (Guid id, OrdersContext dbContext) =>
{
    var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new OrderDetailsDto(
        order.Id,
        order.Reservations,
        order.ScheduledMovieId,
        order.TotalPrice,
        order.OrderState.ToString()));
});

app.Run();
