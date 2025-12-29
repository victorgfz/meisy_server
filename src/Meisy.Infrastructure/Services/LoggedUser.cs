using Meisy.Domain.Entities;
using Meisy.Domain.Security.Token;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

public class LoggedUser : ILoggedUser
{
    private readonly MeisyDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(MeisyDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public int GetUserId()
    {
        var token = _tokenProvider.TokenFromRequest();
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        return int.Parse(jwt.Claims.First(c => c.Type == "UserId").Value);
    }

    public int GetCompanyId()
    {
        var token = _tokenProvider.TokenFromRequest();
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        return int.Parse(jwt.Claims.First(c => c.Type == "CompanyId").Value);
    }

    

    public async Task<User> GetUser()
    {
        var userId = GetUserId();

        return await _dbContext.Users.AsNoTracking().FirstAsync(u => u.Id == userId);
    }
}
