using Microsoft.EntityFrameworkCore;
using MyWebApp.Common;
using MyWebApp.Services;

namespace MyWebApp;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<MyWebAppDbContext>(
            o => o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
        builder.Services.AddTransient<ICompanyService, CompanyService>();
        builder.Services.AddTransient<IMonthRatioProviderService, MonthRatioProviderService>();
        builder.Services.AddTransient<ICSGProvider, CSGProvider>();
        builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        app.Run();
    }
}