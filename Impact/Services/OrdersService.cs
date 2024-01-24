using Impact.Data;
using Impact.Models;
using Microsoft.EntityFrameworkCore;

namespace Impact.Services
{
    public class OrdersService
    {
        private readonly IDataContext dataContext;
        public OrdersService(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Orders>> GetOrders()
        {
            var results = await this.dataContext.Orders.AsNoTracking()
                                                  .ToListAsync();
            return results;
        }

        public async Task<List<Orders>> GetOrdersById(int id)
        {
            var results = await this.dataContext.Orders.AsNoTracking()
                                                .Where(e => e.OrdersId == id)
                                                .ToListAsync();
            return results;
        }

        public async Task CreateOrder(Orders order)
        {
            this.dataContext.Orders.Add(order);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task UpdateOrder(Orders order)
        {
            this.dataContext.Orders.Update(order);
            await this.dataContext.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await this.dataContext.Orders.FindAsync(id);
            this.dataContext.Orders.Remove(order);
            await this.dataContext.SaveChangesAsync();
        }
    }
}