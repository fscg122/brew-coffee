using brew_coffee.Models;
using brew_coffee.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<OpenWeatherOptions>(
    builder.Configuration.GetSection(OpenWeatherOptions.SectionName));
builder.Services.AddSingleton<CoffeeMachineState>();
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddHttpClient<IWeatherService, OpenWeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
