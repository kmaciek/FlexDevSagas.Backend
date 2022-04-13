using FlexDevSagas.Common.Config;
using FlexDevSagas.Services.Movies.Context;
using FlexDevSagas.Services.Movies.Dtos;
using FlexDevSagas.Services.Movies.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<MovieContext>(options =>
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
    var context = services.GetRequiredService<MovieContext>();
    context.Database.Migrate();
}

app.MapGet("/movies", (MovieContext context) =>
{
    return context.Movies.ToList();
});

app.MapGet("/scheduledMovies", (MovieContext context) =>
{
    return context.ScheduledMovies.ToList();
});

app.MapPost("/scheduledMovie", async (ScheduledMovieDto scheduledMovieDto, MovieContext context) =>
{
    var movie = context.Movies.FirstOrDefault(m => m.Id == scheduledMovieDto.MovieId);

    if (movie == null)
    {
        throw new BadHttpRequestException("Invalid movie");
    }

    var scheduleMovie = new ScheduledMovie()
    {
        AuditoriumId = scheduledMovieDto.AuditoriumId,
        End = scheduledMovieDto.End,
        Start = scheduledMovieDto.Start,
        Price = scheduledMovieDto.Price,
        Movie = movie
    };
    context.ScheduledMovies.Add(scheduleMovie);
    await context.SaveChangesAsync();
});
app.Run();
