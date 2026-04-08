using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using brew_coffee.Models;
using brew_coffee.Services;

namespace brew_coffee.Controllers;

[ApiController]
[Route("brew-coffee")]
public class BrewCoffeeController : ControllerBase
{
    private readonly CoffeeMachineState _coffeeMachineState;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IWeatherService _weatherService;

    public BrewCoffeeController(
        CoffeeMachineState coffeeMachineState,
        IDateTimeProvider dateTimeProvider,
        IWeatherService weatherService)
    {
        _coffeeMachineState = coffeeMachineState;
        _dateTimeProvider = dateTimeProvider;
        _weatherService = weatherService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var now = _dateTimeProvider.Now;

        if (now.Month == 4 && now.Day == 1)
        {
            return StatusCode(StatusCodes.Status418ImATeapot);
        }

        var requestCount = _coffeeMachineState.IncrementRequestCount();

        if (requestCount % 5 == 0)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        var temperatureCelsius = await _weatherService.GetCurrentTemperatureCAsync(cancellationToken);
        var message = temperatureCelsius > 30
            ? "Your refreshing iced coffee is ready"
            : "Your piping hot coffee is ready";

        return Ok(new BrewCoffeeResponse(
            message,
            FormatIso8601(now)));
    }

    private static string FormatIso8601(DateTimeOffset dateTimeOffset)
    {
        var offset = dateTimeOffset.ToString("zzz", CultureInfo.InvariantCulture).Replace(":", string.Empty);
        return $"{dateTimeOffset:yyyy-MM-dd'T'HH:mm:ss}{offset}";
    }
}
