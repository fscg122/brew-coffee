namespace brew_coffee.Services;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
