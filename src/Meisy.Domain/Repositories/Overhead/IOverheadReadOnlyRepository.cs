using Meisy.Domain.Enums;

namespace Meisy.Domain.Repositories.Overhead
{
    public interface IOverheadReadOnlyRepository
    {
        Task<bool> OverheadExists(OverheadType type, int companyId);
        Task<List<Domain.Entities.Overhead>> GetAll(int companyId);
        Task<Domain.Entities.Overhead?> GetById(int companyId, int overheadId);
        
    }
}
