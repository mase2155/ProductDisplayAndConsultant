using Group1_VHVL_K64.Models.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Group1_VHVL_K64.Services
{
    /// <summary>
    /// Lưu đơn hàng và gán OrderNumber (1, 2, 3...) liên tục — không phụ thuộc Id identity có khoảng trống.
    /// Dùng transaction mức Serializable + Max(OrderNumber) để hai request đồng thời không trùng số.
    /// </summary>
    public class OrderPersistenceService
    {
        private readonly HoangHoanFurnitureContext _context;

        public OrderPersistenceService(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Thêm đơn vào CSDL, gán OrderNumber = Max(OrderNumber)+1 trong cùng transaction rồi SaveChanges.
        /// </summary>
        public async Task SaveNewOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction transaction =
                    await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

                try
                {
                    int maxSoFar = await _context.Orders
                        .Select(o => (int?)o.OrderNumber)
                        .MaxAsync(cancellationToken) ?? 0;

                    order.OrderNumber = maxSoFar + 1;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}
