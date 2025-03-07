using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Food.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(int id);

        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);

    }
}

