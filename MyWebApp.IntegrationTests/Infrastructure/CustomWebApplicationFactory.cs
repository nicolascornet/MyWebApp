using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyWebApp.Entities;
using MyWebApp.Services;

namespace MyWebApp.IntegrationTests.Infrastructure;


public class CustomWebApplicationFactory<TStartup>
: WebApplicationFactory<TStartup> where TStartup : class
{
    public MockDateTimeProvider DateTimeProvider { get; set; } = new MockDateTimeProvider();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MyWebAppDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<MyWebAppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MyWebAppDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                var csgMock = new Mock<ICSGProvider>();
                csgMock.Setup(x => x.GetExternalFactor())
                       .Returns(5);

                services.AddTransient(sp => csgMock.Object);
                services.AddTransient<IDateTimeProvider>(sp => DateTimeProvider);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                try
                {
                    var company = new Company
                    {
                        Name = "Viseo Test"
                    };
                    db.Companies.Add(company);
                    var employees = Enumerable.Range(1, 1000)
                        .Select(id => new Employee
                        {
                            Name = $"Employee Test #{id}",
                            CompanyId = company.Id,
                            Salary = 100.0m
                        })
                        .ToList();
                    db.Employees.AddRange(employees);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }
}

