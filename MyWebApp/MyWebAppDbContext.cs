using Microsoft.EntityFrameworkCore;
using MyWebApp.Entities;

namespace MyWebApp;

public class MyWebAppDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public MyWebAppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(builder =>
        {
            builder.ToTable("Companies");
            builder.HasMany(c => c.Employees)
                   .WithOne()
                   .HasForeignKey(e => e.CompanyId)
                   .IsRequired();

            //builder.HasData(new Company
            //{
            //    Id = 1,
            //    Name = "Viseo"
            //});
        });

        modelBuilder.Entity<Employee>(builder =>
        {
            builder.ToTable("Employees");
            //var employees = Enumerable.Range(1, 1000)
            //    .Select(id => new Employee
            //    {
            //        Id = id,
            //        Name = $"Employee #{id}",
            //        CompanyId = 1,
            //        Salary = 100.0m
            //    })
            //    .ToList();
            //builder.HasData(employees);
            builder.Property(e => e.Salary)
                   .HasColumnType("decimal(18,4)");
        });

        base.OnModelCreating(modelBuilder);
    }
}
