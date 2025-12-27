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
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }
    }
}
