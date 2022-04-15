using FlexDevSagas.Common.Config;
using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Booking.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
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

        var endpointNameFormatter = x.GetRequiredService<IEndpointNameFormatter>();
        EndpointConvention.Map<GetAuditoriumDetailsRequest>(new Uri($"queue:{endpointNameFormatter.Message<GetAuditoriumDetailsRequest>()}"));
        EndpointConvention.Map<GetScheduledMovieDetailsRequest>(new Uri($"queue:{endpointNameFormatter.Message<GetScheduledMovieDetailsRequest>()}"));
        EndpointConvention.Map<GetScheduledMovieDetailsResponse>(new Uri($"queue:{endpointNameFormatter.Message<GetScheduledMovieDetailsResponse>()}"));
        EndpointConvention.Map<GetScheduledMovieDetailsResponse>(new Uri($"queue:{endpointNameFormatter.Message<GetScheduledMovieDetailsResponse>()}"));

        y.ConfigureEndpoints(x);
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BookingContext>();
    context.Database.Migrate();
}

app.MapGet("/getScheduledMovieBookingDetails/{scheduledMovieId:guid}", async (
    Guid scheduledMovieId,
    BookingContext context,
    IRequestClient<GetScheduledMovieDetailsRequest> scheduledMovieDetailsRequestClient,
    IRequestClient<GetAuditoriumDetailsRequest> auditoriumDetailsRequestClient) =>
{
    var reservations = context.Reservations.Where(r => r.MovieId == scheduledMovieId).ToList();

    var movieRequest = new GetScheduledMovieDetailsRequest(scheduledMovieId);
    var movieDetails =
        await scheduledMovieDetailsRequestClient.GetResponse<GetScheduledMovieDetailsResponse>(movieRequest);

    var auditoriumDetailsRequest = new GetAuditoriumDetailsRequest(movieDetails.Message.AuditoriumId);
    var auditoriumDetails =
        await auditoriumDetailsRequestClient.GetResponse<GetAuditoriumDetailsResponse>(auditoriumDetailsRequest);

    foreach (var row in auditoriumDetails.Message.Rows)
    {
        foreach (var seat in row.Seats)
        {
            var reservation = reservations.FirstOrDefault(r => r.SeatId == seat.Id);
            if (reservation != null)
            {
                seat.IsReserved = true;
            }
        }
    }

    return new
    {
        movieDetails.Message.MovieName,
        movieDetails.Message.Start,
        movieDetails.Message.End,
        movieDetails.Message.Price,
        AuditoriumNumber = auditoriumDetails.Message.Number,
        AuditoriumCapacity = auditoriumDetails.Message.Capacity,
        ReservedSeats = reservations.Count,
        auditoriumDetails.Message.Rows
    };
});
app.Run();
