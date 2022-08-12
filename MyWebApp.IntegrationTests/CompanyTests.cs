using MyWebApp.Entities;
using MyWebApp.IntegrationTests.Infrastructure;
using Shouldly;
using System.Net;

namespace MyWebApp.IntegrationTests
{
    public class CompanyTests
    {
        private CustomWebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        public CompanyTests()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _httpClient = _factory.CreateDefaultClient();
        }

        [Fact]
        public async Task Should_Get_Companies()
        {
            var resp = await _httpClient.GetFromJsonAsync<IEnumerable<Company>>("/company");

            resp.Count().ShouldBe(1);
            resp.First().Name.ShouldBe("Viseo Test");
        }

        [Fact]
        public async Task Wrong_company_should_get_404()
        {
            var ex = Should.Throw<HttpRequestException>(async () =>
            {
                var resp = await _httpClient.GetFromJsonAsync<Company>("/company/2");
            });

            ex.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }



        [Fact]
        public async Task Should_Get_CompanyCharges()
        {
            _factory.DateTimeProvider.Now = new DateTime(2022, 1, 1);
            var resp = await _httpClient.GetFromJsonAsync<decimal>("/company/1/charges");

            resp.ShouldBe(105000);
        }
    }
}
