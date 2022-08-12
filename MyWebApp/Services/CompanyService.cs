using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly MyWebAppDbContext _context;
        private readonly ILogger<CompanyService> _logger;
        private readonly IMonthRatioProviderService _monthRatioProviderService;
        private readonly ICSGProvider _csgProvider;

        public CompanyService(MyWebAppDbContext context, ILogger<CompanyService> logger, 
            IMonthRatioProviderService monthRatioProviderService,
            ICSGProvider csgProvider)
        {
            _context = context;
            _logger = logger;
            _monthRatioProviderService = monthRatioProviderService;
            _csgProvider = csgProvider;
        }

        public async Task<decimal> GetCompanyCharges(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Computing company charges");
            var employeesSalaries = await _context.Employees
                .Where(e => e.CompanyId == id)
                .Select(e => e.Salary)
                .ToListAsync(cancellationToken);

            var ratio = _monthRatioProviderService.GetCurrentMonthRatio();
            var csg = _csgProvider.GetExternalFactor() * employeesSalaries.Count;
            return employeesSalaries.Sum(x => x * (decimal)ratio) + csg;
        }
    }
}
