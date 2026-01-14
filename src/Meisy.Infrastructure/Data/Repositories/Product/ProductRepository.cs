using Meisy.Domain.Entities;
using Meisy.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Product
{
    public class ProductRepository : IProductReadOnlyRepository, IProductWriteOnlyRepository
    {
        private readonly MeisyDbContext _dbContext;
        public ProductRepository(MeisyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Domain.Entities.Product product)
        {
            await _dbContext.Products.AddAsync(product);
        }

       

        public async Task<List<Domain.Entities.Product>> GetAll(int companyId)
        {
            return await _dbContext.Products.AsNoTracking().Where(p => p.CompanyId == companyId).ToListAsync();
        }

        public async Task<Domain.Entities.Product?> GetById(int companyId, int productId)
        {
            return await
                _dbContext.Products
                .AsNoTracking()
                .Include(p => p.ProductInputs)
                .ThenInclude(p => p.Input)
                .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == productId);
        }
        public async Task<Domain.Entities.Product?> GetByIdForUpdate(int companyId, int productId)
        {
            return await
                _dbContext.Products
                .Include(p => p.ProductInputs)
                .ThenInclude(p => p.Input)
                .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == productId);
        }



        public void Delete(Domain.Entities.Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public void DeleteProductInputs(List<ProductInput> productInputs)
        {
            foreach(var item in productInputs)
            {
                _dbContext.Product_Inputs.Remove(item);
            }
        }

        

        

        
    }
}
