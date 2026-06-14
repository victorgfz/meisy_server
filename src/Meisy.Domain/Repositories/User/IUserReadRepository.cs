using Meisy.Domain.Entities;

namespace Meisy.Domain.Repositories.User
{
    public interface IUserReadRepository
    {

        Task<bool> EmailExists(string email);
        Task<Meisy.Domain.Entities.User?> GetByEmail(string email);
        Task<Meisy.Domain.Entities.User?> GetById(int companyId,int userId);
        Task<Meisy.Domain.Entities.User?> GetByRefreshToken(string refreshToken);
    }
}
