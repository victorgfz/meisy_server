namespace Meisy.Domain.Repositories.User
{
    public interface IUserWriteRepository
    {
        Task Add(Meisy.Domain.Entities.User user);
        
    }
}
