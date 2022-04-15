using FlexDevSagas.Common.Config;
using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Cinemas.Consumers;
using FlexDevSagas.Services.Cinemas.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<CinemaContext>(options =>
    options.UseSqlServer(connectionString));

var rabbitMqConfig = new RabbitMQConfig();
builder.Configuration.Bind("RabbitMQConfig", rabbitMqConfig);
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();
    cfg.AddConsumersFromNamespaceContaining<ConsumerAnchor>();
    cfg.UsingRabbitMq((x, y) =>
    {
        y.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h =>
        {
            h.Username(rabbitMqConfig.Username);
            h.Password(rabbitMqConfig.Password);
        });

        var endpointNameFormatter = x.GetRequiredService<IEndpointNameFormatter>();
        EndpointConvention.Map<GetAuditoriumDetailsRequest>(new Uri($"queue:{endpointNameFormatter.Consumer<GetAuditoriumDetailsRequestConsumer>()}"));
        EndpointConvention.Map<GetAuditoriumDetailsResponse>(new Uri($"queue:{endpointNameFormatter.Message<GetAuditoriumDetailsResponse>()}"));

        y.ConfigureEndpoints(x);
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CinemaContext>();
    context.Database.Migrate();
    CinemaSeeds.Seed(context);
}

app.MapGet("/cinemas", (CinemaContext context) =>
{
    return context.Cinemas
        .Include(c => c.Auditoriums)
        .ThenInclude(a => a.Rows)
        .ThenInclude(r => r.Seats)
        .ToList();
});
app.MapGet("/cinemas/{id:guid}", (Guid id, CinemaContext context) =>
{
    return context.Cinemas
        .Include(c => c.Auditoriums)
        .ThenInclude(a => a.Rows)
        .ThenInclude(r => r.Seats)
        .FirstOrDefault(c => c.Id == id);
});
app.MapGet("/auditoriums", (CinemaContext context) => context.Auditoriums.Include(a => a.Rows).ToList());

app.Run();