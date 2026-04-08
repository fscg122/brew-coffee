using brew_coffee.Models;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace brew_coffee.Services;

public sealed class OpenWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    public OpenWeatherService(HttpClient httpClient, IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<double> GetCurrentTemperatureCAsync(CancellationToken cancellationToken = default)
    {
        ValidateOptions();

        var requestUri = string.Format(
            CultureInfo.InvariantCulture,
            "weather?lat={0}&lon={1}&appid={2}&units=metric",
            _options.Latitude,
            _options.Longitude,
            Uri.EscapeDataString(_options.ApiKey));

        var response = await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(requestUri, cancellationToken);

        if (response?.Main is null)
        {
            throw new InvalidOperationException("The weather service returned an unexpected response.");
        }

        return response.Main.Temp;
    }

    private void ValidateOptions()
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("OpenWeather ApiKey must be configured.");
        }
    }

    private sealed class OpenWeatherResponse
    {
        public MainWeatherData? Main { get; init; }
    }

    private sealed class MainWeatherData
    {
        public double Temp { get; init; }
    }
}
