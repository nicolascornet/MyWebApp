namespace MyWebApp.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}