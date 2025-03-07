using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO : SingletonBase<OrderDAO>
    {

        public async Task<IEnumerable<Order>> GetOrderAll()
        {
            return await _context.Orders
                .Include(o => o.Product) // Tải thông tin Product liên quan
                .Include(o => o.User)    // Tải thông tin User liên quan
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderDetailsId == id);
            return order;
        }
        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Product) 
                .ToListAsync();
        }

        public async Task Add(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Order order)
        {
            var existingItem = await GetOrderById(order.OrderDetailsId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(order);
            }
            else
            {
                await _context.Orders.AddAsync(order);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await GetOrderById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

		public async Task<int> GetOrderCount()
		{
			return await _context.Orders.CountAsync();
		}

	}
}