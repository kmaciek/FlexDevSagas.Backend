using FlexDevSagas.Common.Config;
using FlexDevSagas.Services.Booking.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<BookingContext>(options =>
    options.UseSqlServer(connectionString));

var rabbitMqConfig = new RabbitMQConfig();
builder.Configuration.Bind("RabbitMQConfig", rabbitMqConfig);
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    //cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
    cfg.UsingRabbitMq((x, y) =>
    {
        y.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h =>
        {
            h.Username(rabbitMqConfig.Username);
            h.Password(rabbitMqConfig.Password);
        });

        //var endpointNameFormatter = x.GetRequiredService<IEndpointNameFormatter>();
        //EndpointConvention.Map<SendMessageEvent>(new Uri($"queue:{endpointNameFormatter.Consumer<SendMessageEventConsumer>()}"));
        //EndpointConvention.Map<UserCreateCompletedEvent>(new Uri($"queue:{endpointNameFormatter.Message<UserCreateCompletedEvent>()}"));
        //EndpointConvention.Map<ProcessEmailConfirmationSendEvent>(new Uri($"queue:{endpointNameFormatter.Consumer<ProcessEmailConfirmationSendEventConsumer>()}"));

        y.ConfigureEndpoints(x);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BookingContext>();
    context.Database.Migrate();
}

app.MapGet("/", () => "Hello World!");

app.Run();
