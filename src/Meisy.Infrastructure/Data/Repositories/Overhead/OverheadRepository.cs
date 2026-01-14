using Meisy.Domain.Enums;
using Meisy.Domain.Repositories.Overhead;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Overhead
{
    public class OverheadRepository : IOverheadReadOnlyRepository, IOverheadWriteOnlyRepository
    {
        private readonly MeisyDbContext _dbContext;
        public OverheadRepository(
            MeisyDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task<bool> OverheadExists(OverheadType type, int companyId)
        {
            return await _dbContext.Overheads.AnyAsync(o => o.Type == type && o.CompanyId == companyId);
        }
        
        public async Task Add(Domain.Entities.Overhead overhead)
        {
            await _dbContext.Overheads.AddAsync(overhead);
        }

        public async Task<List<Domain.Entities.Overhead>> GetAll(int companyId)
        {
            return await _dbContext.Overheads.AsNoTracking().Where(o => o.CompanyId == companyId).ToListAsync();
        }

        public async Task<Domain.Entities.Overhead?> GetById(int companyId, int overheadId)
        {
            return await _dbContext.Overheads.FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == overheadId);
        }

        

    }
}
