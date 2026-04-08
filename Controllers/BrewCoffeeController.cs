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

    public BrewCoffeeController(CoffeeMachineState coffeeMachineState, IDateTimeProvider dateTimeProvider)
    {
        _coffeeMachineState = coffeeMachineState;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpGet]
    public IActionResult Get()
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

        return Ok(new BrewCoffeeResponse(
            "Your piping hot coffee is ready",
            FormatIso8601(now)));
    }

    private static string FormatIso8601(DateTimeOffset dateTimeOffset)
    {
        var offset = dateTimeOffset.ToString("zzz", CultureInfo.InvariantCulture).Replace(":", string.Empty);
        return $"{dateTimeOffset:yyyy-MM-dd'T'HH:mm:ss}{offset}";
    }
}
