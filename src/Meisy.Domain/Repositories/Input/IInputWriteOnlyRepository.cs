namespace Meisy.Domain.Repositories.Input
{
    public interface IInputWriteOnlyRepository
    {
        Task Add(Domain.Entities.Input input); 
        void Delete(Domain.Entities.Input input); 
        void Update(Domain.Entities.Input input);
    }
}
