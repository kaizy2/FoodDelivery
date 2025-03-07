using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Security.Claims;

namespace Food.Pages.Users
{
    public class DefaultModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        public IEnumerable<Product> Products { get; set; }

        public DefaultModel(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;

        }


        public async Task OnGet()
        {
            // Fetch all products from the database
            Products = await _productRepository.GetAllProducts();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                return RedirectToPage("/Users/Login"); // Chuyển hướng nếu chưa đăng nhập
            }

            var existingCartItem = await _cartRepository.GetCartItemAsync(parsedUserId, ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                await _cartRepository.Update(existingCartItem);
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = parsedUserId,
                    ProductId = ProductId,
                    Quantity = 1
                };
                await _cartRepository.Add(cartItem);
            }

            return RedirectToPage("/Users/Carts"); // Chuyển đến giỏ hàng sau khi thêm thành công
        }
    }
    }
