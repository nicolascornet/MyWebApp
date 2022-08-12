namespace MyWebApp.Services
{
    public interface ICompanyService
    {
        Task<decimal> GetCompanyCharges(int id, CancellationToken cancellationToken);
    }
}