using FlexDevSagas.Common.Config;
using FlexDevSagas.Common.Message;
using FlexDevSagas.Common.Requests;
using FlexDevSagas.Common.Responses;
using FlexDevSagas.Services.Movies.Consumers;
using FlexDevSagas.Services.Movies.Context;
using FlexDevSagas.Services.Movies.Dtos;
using FlexDevSagas.Services.Movies.Entities;
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
builder.Services.AddDbContext<MovieContext>(options =>
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
        EndpointConvention.Map<GetScheduledMovieDetailsRequest>(new Uri($"queue:{endpointNameFormatter.Consumer<GetScheduledMovieDetailsRequestConsumer>()}"));
        EndpointConvention.Map<GetScheduledMovieDetailsResponse>(new Uri($"queue:{endpointNameFormatter.Message<GetScheduledMovieDetailsResponse>()}"));
        EndpointConvention.Map<GetScheduledMoviesDetailsRequest>(new Uri($"queue:{endpointNameFormatter.Consumer<GetScheduledMovieDetailsRequestConsumer>()}"));
        EndpointConvention.Map<GetScheduledMoviesDetailsResponse>(new Uri($"queue:{endpointNameFormatter.Message<GetScheduledMoviesDetailsResponse>()}"));

        y.ConfigureEndpoints(x);
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MovieContext>();
    context.Database.Migrate();
    MovieSeeds.Seed(context);
}

app.MapGet("/movies", (MovieContext context) => context.Movies.ToList());

app.MapGet("/scheduledMovies", (MovieContext context) => context.ScheduledMovies.Include(sm => sm.Movie).OrderBy(sm => sm.Start).ToList());

app.MapGet("/scheduledMovies/findByCinema/{id:guid}",
    (Guid id, MovieContext context) =>
    {
        return context.ScheduledMovies
            .Include(sm => sm.Movie)
            .Where(sm => sm.CinemaId == id)
            .OrderBy(sm => sm.Start)
            .Select(sm => new GetScheduledMovieDto(
                sm.Id,
                sm.AuditoriumId,
                sm.CinemaId,
                TimeZoneInfo.ConvertTimeFromUtc(sm.Start, TimeZoneInfo.Local),
                TimeZoneInfo.ConvertTimeFromUtc(sm.End, TimeZoneInfo.Local),
                sm.Price,
                new MovieDto(sm.Movie.Id, sm.Movie.Name)
            ));
    });

app.MapPost("/scheduledMovies", async (PostScheduledMovieDto scheduledMovieDto, MovieContext context) =>
{
    var movie = context.Movies.FirstOrDefault(m => m.Id == scheduledMovieDto.MovieId);

    if (movie == null)
    {
        throw new BadHttpRequestException("Invalid movie");
    }

    var scheduleMovie = new ScheduledMovie()
    {
        AuditoriumId = scheduledMovieDto.AuditoriumId,
        End = TimeZoneInfo.ConvertTimeToUtc(scheduledMovieDto.End),
        Start = TimeZoneInfo.ConvertTimeToUtc(scheduledMovieDto.Start),
        Price = scheduledMovieDto.Price,
        Movie = movie,
        CinemaId = scheduledMovieDto.CinemaId
    };
    context.ScheduledMovies.Add(scheduleMovie);
    await context.SaveChangesAsync();
});
app.Run();
