using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CartDAO : SingletonBase<CartDAO>
    {
        public async Task<IEnumerable<Cart>> GetCartAll()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetCartById(int id)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == id);
            return cart;
        }
		public async Task<Cart> GetCartItemAsync(int userId, int productId)
		{
			
			var cartItem = await _context.Carts
										 .Include(c => c.Product)  
										 .Where(c => c.UserId == userId && c.ProductId == productId)
										 .FirstOrDefaultAsync();

			return cartItem;
		}

		public async Task<List<Cart>> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                                 .Include(c => c.Product)
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }

        public async Task Add(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Cart cart)
        {
            var existingItem = await GetCartById(cart.CartId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(cart);
            }
            else
            {
                _context.Carts.Add(cart);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var cart = await GetCartById(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}
