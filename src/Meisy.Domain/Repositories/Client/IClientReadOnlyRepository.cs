namespace Meisy.Domain.Repositories.Client
{
    public interface IClientReadOnlyRepository
    {
        Task<List<Domain.Entities.Client>> GetAll(int companyId);
        Task<Domain.Entities.Client?> GetByIdForUpdate(int companyId, int clientId);
        Task<Domain.Entities.Client?> GetById(int companyId, int clientId);
    }
}
