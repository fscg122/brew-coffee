namespace brew_coffee.Models;

public sealed class OpenWeatherOptions
{
    public const string SectionName = "OpenWeather";

    public string ApiKey { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
