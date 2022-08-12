using MyWebApp.Services;

namespace MyWebApp.IntegrationTests
{
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; } = DateTime.Now;
    }
}
