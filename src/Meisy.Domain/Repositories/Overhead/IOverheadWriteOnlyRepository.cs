namespace Meisy.Domain.Repositories.Overhead
{
    public interface IOverheadWriteOnlyRepository
    {
        Task Add(Domain.Entities.Overhead overhead);
        void Update(Domain.Entities.Overhead overhead);
    }
}
