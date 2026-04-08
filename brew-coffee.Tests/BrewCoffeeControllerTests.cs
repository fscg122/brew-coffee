using brew_coffee.Controllers;
using brew_coffee.Models;
using brew_coffee.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brew_coffee.Tests;

public class BrewCoffeeControllerTests
{
    [Fact]
    public void Get_ReturnsCoffeeResponse_WhenMachineCanBrew()
    {
        var now = new DateTimeOffset(2021, 2, 3, 11, 56, 24, TimeSpan.FromHours(9));
        var controller = CreateController(now);

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BrewCoffeeResponse>(okResult.Value);
        Assert.Equal("Your piping hot coffee is ready", response.Message);
        Assert.Equal("2021-02-03T11:56:24+0900", response.Prepared);
    }

    [Fact]
    public void Get_ReturnsServiceUnavailable_OnEveryFifthRequest()
    {
        var controller = CreateController(new DateTimeOffset(2021, 2, 3, 11, 56, 24, TimeSpan.FromHours(9)));

        for (var index = 0; index < 4; index++)
        {
            controller.Get();
        }

        var result = controller.Get();

        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, statusCodeResult.StatusCode);
    }

    [Fact]
    public void Get_ReturnsTeapot_OnAprilFirst()
    {
        var controller = CreateController(new DateTimeOffset(2021, 4, 1, 9, 0, 0, TimeSpan.FromHours(9)));

        var result = controller.Get();

        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status418ImATeapot, statusCodeResult.StatusCode);
    }

    private static BrewCoffeeController CreateController(DateTimeOffset now)
    {
        return new BrewCoffeeController(
            new CoffeeMachineState(),
            new FakeDateTimeProvider(now));
    }

    private sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        public FakeDateTimeProvider(DateTimeOffset now)
        {
            Now = now;
        }

        public DateTimeOffset Now { get; }
    }
}
