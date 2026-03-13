using Ordering.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Core.Repositories
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
