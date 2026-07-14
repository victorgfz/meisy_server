using Meisy.Domain.Models;
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
                .Where(o => o.CompanyId == companyId && o.Status != Domain.Enums.OrderStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Order?> GetByIdForUpdate(int companyId, int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == orderId);
        }

        public async Task<List<Domain.Entities.Order>> GetAllByMonth(int companyId, DateTime date)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.OrderProducts)
                .ThenInclude(o => o.Product)
                .Where(o => o.CompanyId == companyId && date.Month == o.DeliveryDate.Month && date.Year == o.DeliveryDate.Year && o.Status != Domain.Enums.OrderStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<List<Domain.Entities.Order>> GetPendingOrdersForDeliveryReminder(DateTime now, DateTime reminderLimit)
        {
            return await _dbContext.Orders
                .Where(o => o.Status == Domain.Enums.OrderStatus.Pending
                    && o.DeliveryReminderSentAt == null
                    && o.DeliveryDate > now
                    && o.DeliveryDate <= reminderLimit)
                .ToListAsync();
        }

        public async Task<List<OrderProductIncidence>> GetTopProductsByMonth(int companyId, DateTime date, int top = 3)
        {
            return await _dbContext.Order_Products
                .AsNoTracking()
                .Where(op => op.Order.CompanyId == companyId && op.Order.DeliveryDate.Month == date.Month && op.Order.DeliveryDate.Year == date.Year && op.Order.Status == Domain.Enums.OrderStatus.Completed)
                .GroupBy(op => new {op.ProductId, op.Product.Description})
                .Select(it => new OrderProductIncidence {
                    ProductId = it.Key.ProductId,
                    Description = it.Key.Description,
                    Total = it.Sum(op => op.Amount),
                    TotalRevenue = it.Sum(op => op.PriceAtTheMoment * op.Amount)})
                .OrderByDescending(it => it.Total)
                .Take(top)
                .ToListAsync();
        }
    }
}
