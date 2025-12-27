using Meisy.Domain.Repositories;

namespace Meisy.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MeisyDbContext _dbContext;
        public UnitOfWork(MeisyDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
