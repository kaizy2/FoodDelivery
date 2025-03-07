using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Food.Repositories;
using Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Food.Pages.Users
{
    public class MenuModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public MenuModel(ICategoryRepository categoryRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        public int SelectedCategoryId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadCategoriesAsync();
            await LoadAllProductsAsync();
        }

        public async Task<IActionResult> OnGetShowAllProductsAsync()
        {
            SelectedCategoryId = 0;
            await LoadAllProductsAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetFilterByCategoryAsync(int categoryId)
        {
            SelectedCategoryId = categoryId;
            await LoadProductsByCategoryAsync(categoryId);
            return Page();
        }

        // New handler to add a product to the cart
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                // Handle the case where userId is invalid
                return RedirectToPage("/Users/Login"); // Redirect to login if user is not authenticated
            }

            var existingCartItem = await _cartRepository.GetCartItemAsync(parsedUserId, productId);

            if (existingCartItem != null)
            {
                // If the item is already in the cart, increase the quantity
                existingCartItem.Quantity += 1;
                await _cartRepository.Update(existingCartItem);
            }
            else
            {
                // If the item is not in the cart, create a new cart item
                var cartItem = new Cart
                {
                    UserId = parsedUserId,
                    ProductId = productId,
                    Quantity = 1
                };
                await _cartRepository.Add(cartItem);
            }

            return RedirectToPage("/Users/Carts"); // Redirect to the cart page after adding the product
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategories();
            Categories = categories.ToList();
        }

        private async Task LoadAllProductsAsync()
        {
            var products = await _productRepository.GetAllProducts();
            Products = products.ToList();
        }

        private async Task LoadProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategory(categoryId);
            Products = products.ToList();
        }
    }
}



