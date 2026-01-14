using Meisy.Domain.Entities;
using Meisy.Domain.Enums;
using Meisy.Domain.Repositories.Input;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Input
{
    public class InputRepository : IInputReadOnlyRepository, IInputWriteOnlyRepository
    {
        private readonly MeisyDbContext _dbContext;

        public InputRepository(MeisyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Domain.Entities.Input input)
        {
            await _dbContext.Inputs.AddAsync(input);
        }

        

        public async Task<List<Domain.Entities.Input>> GetAllByType(int companyId, InputType type)
        {
            return await _dbContext.Inputs.AsNoTracking().Where(i => i.CompanyId == companyId && i.Type == type).ToListAsync();
        }

        public async Task<Domain.Entities.Input?> GetById(int companyId, int inputId)
        {
            return await _dbContext.Inputs.FirstOrDefaultAsync(i => i.CompanyId == companyId && i.Id == inputId);
        }

        
        
        public void Delete(Domain.Entities.Input input)
        {
            _dbContext.Inputs.Remove(input);
        }
    }
}
