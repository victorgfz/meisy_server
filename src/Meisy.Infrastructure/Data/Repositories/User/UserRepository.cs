using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.User
{
    public class UserRepository : IUserWriteRepository, IUserReadRepository
    {
        private readonly MeisyDbContext _dbContext;
        public UserRepository(
            MeisyDbContext dbContext
            )
        {
            _dbContext= dbContext;
        }

        public async Task Add(Domain.Entities.User user)
        {   
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<Domain.Entities.User?> GetByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking().Include(u => u.Company).FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<Domain.Entities.User?> GetById(int companyId, int userId)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId && user.CompanyId == companyId);

        }

        public void Update(Domain.Entities.User user)
        {
            _dbContext.Users.Update(user);
        }

        public async Task<Domain.Entities.User?> GetByRefreshToken(string refreshToken)
        {
            return await _dbContext.Users.Include(u => u.Company).FirstOrDefaultAsync(user => user.RefreshToken != null && user.RefreshToken.Equals(refreshToken));
        }
    }
}
