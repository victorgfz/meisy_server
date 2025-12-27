using Meisy.Domain.Repositories.Company;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Company
{
    public class CompanyRepository : ICompanyWriteRepository, ICompanyReadRepository
    {
        private readonly MeisyDbContext _dbContext;
        public CompanyRepository(MeisyDbContext dbContext)
        {   
            _dbContext = dbContext;
        }


        public async Task Add(Domain.Entities.Company company)
        {
            
            await _dbContext.Companies.AddAsync(company);
        }

        public async Task<Domain.Entities.Company> GetByCode(string code)
        {
            return await _dbContext.Companies.AsNoTracking().FirstAsync(company => company.Code.Equals(code));
        }

        public async Task<bool> CompanyExists(string code)
        {
            return await _dbContext.Companies.AnyAsync(company => company.Code.Equals(code));
        }
    }
}
