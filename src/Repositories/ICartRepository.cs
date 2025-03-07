using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.Repositories
{
    public interface ICartRepository
    {
		Task<Cart> GetCartItemAsync(int userId, int productId);
		Task<IEnumerable<Cart>> GetAllCarts();
        Task<Cart> GetCartById(int id);
        Task<List<Cart>> GetCartByUserIdAsync(int userId);
        Task Add(Cart cart);
        Task Update(Cart cart);
        Task Delete(int id);
    }
}
