using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Entities;
using MyWebApp.Exceptions;
using MyWebApp.Services;

namespace MyWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{

    private readonly ILogger<CompanyController> _logger;
    private readonly MyWebAppDbContext _context;
    private readonly ICompanyService _companyService;

    public CompanyController(ILogger<CompanyController> logger, MyWebAppDbContext context, ICompanyService companyService)
    {
        _logger = logger;
        _context = context;
        _companyService = companyService;
    }

    [HttpGet(Name = "GetAllCompanies")]
    public async Task<IEnumerable<Company>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Companies
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
    }

    [HttpGet("{id}", Name = "GetCompany")]
    public async Task<Company> Get(int id, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
        {
            throw new NotFoundException(nameof(Company), id);
        }
        return company;
    }

    [HttpGet("{id}/charges", Name = "GetCompanyCharges")]
    public async Task<decimal> GetCharges(int id, CancellationToken cancellationToken)
    {
        return await _companyService.GetCompanyCharges(id, cancellationToken);
    }

    /// <summary>
    /// Increase employees salary by 10%
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPut("{id}", Name = "IncreaseSalary")]
    public async Task<IActionResult> IncreaseSalary(int id, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            //.Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
        {
            throw new NotFoundException(nameof(Company), id);
        }


        await _context.Database.BeginTransactionAsync(cancellationToken);

        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE Employees SET Salary = Salary * 1.1 WHERE CompanyId = {company.Id}", cancellationToken);

        //foreach (var employee in company.Employees)
        //{
        //    employee.Salary *= 1.1m;
        //}
        company.LastSalaryUpdateUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Database.CommitTransactionAsync(cancellationToken);

        return new NoContentResult();
    }
}
