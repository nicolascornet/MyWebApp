using MyWebApp.Services;
using Shouldly;

namespace MyWebApp.UnitTests
{
    public class MonthRatioProviderTests
    {
        [Fact]
        public void Ratio_Should_Be_One_in_January()
        {
            //arrange
            var dtp = new MockDateTimeProvider()
            {
                Now = new DateTime(2022, 1, 1)
            };
            var mrp = new MonthRatioProviderService(dtp);

            var result = mrp.GetCurrentMonthRatio();

            result.ShouldBe(1.0);
        }
    }
}