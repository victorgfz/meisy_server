using Meisy.Domain.Entities;

namespace Meisy.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {

        Task<User> GetUser();
        int GetUserId();
        int GetCompanyId();
    }
}
