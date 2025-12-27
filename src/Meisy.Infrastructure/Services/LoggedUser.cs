using Meisy.Domain.Entities;
using Meisy.Domain.Security.Token;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Meisy.Infrastructure.Services
{
    public class LoggedUser : ILoggedUser
    {

        private readonly MeisyDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(MeisyDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> Get()
        {

            string token = _tokenProvider.TokenFromRequest();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Id == int.Parse(identifier));
        }
    }
}
