using Meisy.Domain.Enums;

namespace Meisy.Domain.Repositories.Input
{
    public interface IInputReadOnlyRepository
    {
        Task<List<Domain.Entities.Input>> GetAllByType(int companyId, InputType type);
        Task<Domain.Entities.Input?> GetById(int companyId, int inputId);
    }
}
