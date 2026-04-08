namespace brew_coffee.Services;

public interface IWeatherService
{
    Task<double> GetCurrentTemperatureCAsync(CancellationToken cancellationToken = default);
}
