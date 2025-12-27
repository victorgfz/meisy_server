namespace Meisy.Domain.Repositories.Company
{
    public interface ICompanyWriteRepository
    {
        Task Add(Meisy.Domain.Entities.Company company);
    }
}
