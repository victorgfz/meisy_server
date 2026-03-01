using Meisy.Domain.Repositories.Order;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.Order
{
    public class OrderRepository : IOrderWriteOnlyRepository, IOrderReadOnlyRepository
    {
        private readonly MeisyDbContext _dbContext;

        public OrderRepository(MeisyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Domain.Entities.Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public async Task<List<Domain.Entities.Order>> GetAll(int companyId)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.Product)
                .Where(o => o.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Order?> GetByIdForUpdate(int companyId, int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == orderId);
        }
    }
}
