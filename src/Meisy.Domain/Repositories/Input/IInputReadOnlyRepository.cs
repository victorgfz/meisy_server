using Meisy.Domain.Enums;

namespace Meisy.Domain.Repositories.Input
{
    public interface IInputReadOnlyRepository
    {
        Task<List<Domain.Entities.Input>> GetAll(int companyId);
        Task<Domain.Entities.Input?> GetById(int companyId, int inputId);
        Task<bool> IsInputBeingUsed(int companyId, int inputId);
    }
}
