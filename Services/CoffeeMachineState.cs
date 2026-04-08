namespace brew_coffee.Services;

public sealed class CoffeeMachineState
{
    private int _requestCount;

    public int IncrementRequestCount()
    {
        return Interlocked.Increment(ref _requestCount);
    }
}
