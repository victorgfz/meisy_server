using Meisy.Domain.Entities;

namespace Meisy.Domain.Security.Token
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
