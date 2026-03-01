namespace Meisy.Domain.Repositories.Client
{
    public interface IClientWriteOnlyRepository
    {
        Task Add(Domain.Entities.Client client);
        void Delete(Domain.Entities.Client client);
    }
}
