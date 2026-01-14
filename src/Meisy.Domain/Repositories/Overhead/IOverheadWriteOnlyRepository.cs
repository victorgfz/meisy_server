namespace Meisy.Domain.Repositories.Overhead
{
    public interface IOverheadWriteOnlyRepository
    {
        Task Add(Domain.Entities.Overhead overhead);
    }
}
