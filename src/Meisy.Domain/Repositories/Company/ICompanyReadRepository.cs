namespace Meisy.Domain.Repositories.Company
{
    public interface ICompanyReadRepository
    {
        Task<bool> CompanyExists(string code);
        Task<Domain.Entities.Company> GetByCode(string code);

    }
}
