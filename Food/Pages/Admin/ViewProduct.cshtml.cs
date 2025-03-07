using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Food.Pages.Admin.Product
{
    public class ViewProductModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ViewProductModel(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<ProductViewModel> ProductsList { get; set; }

        public async Task OnGetAsync()
        {
            var products = await _productRepository.GetAllProducts();
            ProductsList = products.Select(p => new ProductViewModel
            {
                ProductID = p.ProductId,
                ProductName = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryName = p.Category != null ? p.Category.Name : "Unknown",
                IsActive = p.IsActive,
                Description = p.Description,
                CreatedDate = p.CreatedDate,
                ImageUrl = p.ImageUrl
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _productRepository.Delete(id);
            return RedirectToPage();
        }
    }

    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
