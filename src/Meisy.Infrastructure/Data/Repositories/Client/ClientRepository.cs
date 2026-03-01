using Meisy.Domain.Repositories.Client;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Client
{
    public class ClientRepository : IClientWriteOnlyRepository, IClientReadOnlyRepository
    {
        private readonly MeisyDbContext _dbContext;
        public ClientRepository(MeisyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Domain.Entities.Client client)
        {
            await _dbContext.Clients.AddAsync(client);
        }

        public void Delete(Domain.Entities.Client client)
        {
            _dbContext.Clients.Remove(client);
        }

        public async Task<List<Domain.Entities.Client>> GetAll(int companyId)
        {
            return await _dbContext.Clients.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        public async Task<Domain.Entities.Client?> GetByIdForUpdate(int companyId, int clientId)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId && c.CompanyId == companyId);
        }
        public async Task<Domain.Entities.Client?> GetById(int companyId, int clientId)
        {
            return await _dbContext.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clientId && c.CompanyId == companyId);
        }
    }
}
