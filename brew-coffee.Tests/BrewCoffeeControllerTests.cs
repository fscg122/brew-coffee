using brew_coffee.Controllers;
using brew_coffee.Models;
using brew_coffee.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brew_coffee.Tests;

public class BrewCoffeeControllerTests
{
    [Fact]
    public async Task Get_ReturnsHotCoffeeResponse_WhenTemperatureIsThirtyOrBelow()
    {
        var now = new DateTimeOffset(2021, 2, 3, 11, 56, 24, TimeSpan.FromHours(9));
        var controller = CreateController(now, 30);

        var result = await controller.Get(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BrewCoffeeResponse>(okResult.Value);
        Assert.Equal("Your piping hot coffee is ready", response.Message);
        Assert.Equal("2021-02-03T11:56:24+0900", response.Prepared);
    }

    [Fact]
    public async Task Get_ReturnsIcedCoffeeResponse_WhenTemperatureIsAboveThirty()
    {
        var now = new DateTimeOffset(2021, 2, 3, 11, 56, 24, TimeSpan.FromHours(9));
        var controller = CreateController(now, 31);

        var result = await controller.Get(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BrewCoffeeResponse>(okResult.Value);
        Assert.Equal("Your refreshing iced coffee is ready", response.Message);
    }

    [Fact]
    public async Task Get_ReturnsServiceUnavailable_OnEveryFifthRequest()
    {
        var controller = CreateController(new DateTimeOffset(2021, 2, 3, 11, 56, 24, TimeSpan.FromHours(9)), 20);

        for (var index = 0; index < 4; index++)
        {
            await controller.Get(CancellationToken.None);
        }

        var result = await controller.Get(CancellationToken.None);

        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Get_ReturnsTeapot_OnAprilFirst()
    {
        var controller = CreateController(new DateTimeOffset(2021, 4, 1, 9, 0, 0, TimeSpan.FromHours(9)), 35);

        var result = await controller.Get(CancellationToken.None);

        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status418ImATeapot, statusCodeResult.StatusCode);
    }

    private static BrewCoffeeController CreateController(DateTimeOffset now, double temperatureCelsius)
    {
        return new BrewCoffeeController(
            new CoffeeMachineState(),
            new FakeDateTimeProvider(now),
            new FakeWeatherService(temperatureCelsius));
    }

    private sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        public FakeDateTimeProvider(DateTimeOffset now)
        {
            Now = now;
        }

        public DateTimeOffset Now { get; }
    }

    private sealed class FakeWeatherService : IWeatherService
    {
        private readonly double _temperatureCelsius;

        public FakeWeatherService(double temperatureCelsius)
        {
            _temperatureCelsius = temperatureCelsius;
        }

        public Task<double> GetCurrentTemperatureCAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_temperatureCelsius);
        }
    }
}
