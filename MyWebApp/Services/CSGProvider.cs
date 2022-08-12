namespace MyWebApp.Services;

public class CSGProvider : ICSGProvider
{
    private Random _random = new Random();

    public int GetExternalFactor()
    {
        return _random.Next(100);
    }
}
