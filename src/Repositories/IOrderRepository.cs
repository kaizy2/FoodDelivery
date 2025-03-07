using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(int id);

        Task<int> GetOrderCount();

	}
}
