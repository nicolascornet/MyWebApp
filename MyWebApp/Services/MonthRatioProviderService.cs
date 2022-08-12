namespace MyWebApp.Services
{
    public class MonthRatioProviderService : IMonthRatioProviderService
    {
        public MonthRatioProviderService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        private Dictionary<int, double> _monthRatio = new ()
        {
            { 1, 1.0 }, //January 
            { 2, 1.0 }, //February 
            { 3, 1.0 }, //March 
            { 4, 1.0 }, //April 
            { 5, 1.0 }, //May
            { 6, 1.0 }, //June
            { 7, 1.3 }, //July
            { 8, 1.0 }, //August
            { 9, 1.0 }, //September
            { 10, 1.0 }, //October
            { 11, 1.0 }, //November
            { 12, 1.5 }, //December
        };
        private readonly IDateTimeProvider _dateTimeProvider;

        public double GetCurrentMonthRatio()
        {
            return _monthRatio[_dateTimeProvider.Now.Month];
        }
    }
}
