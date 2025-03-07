using Microsoft.AspNetCore.Mvc;
using Food.Repositories;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Food.Services;
using Net.payOS.Types;

namespace Food.Pages.Users
{
	[Authorize(Roles = "User")]
	public class CartModel : PageModel
    {

        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly PaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;

        public CartModel(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository, PaymentService paymentService, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _paymentService = paymentService;
            _orderRepository = orderRepository;
            Carts = new List<Cart>();
        }

        public List<Cart> Carts { get; set; }
        public decimal TotalPrice { get; set; }
        public User UserProfile { get; set; }
        [BindProperty]
        public int ProductId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Carts = await _cartRepository.GetCartByUserIdAsync(int.Parse(userId));
            TotalPrice = Carts.Sum(c => c.Product.Price * c.Quantity);
            UserProfile = await _userRepository.GetUserById(int.Parse(userId));

            return Page();
        }

        // Action to update the quantity in the cart
        public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = await _cartRepository.GetCartItemAsync(int.Parse(userId), productId);

            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                await _cartRepository.Update(cartItem);
            }

            // Recalculate total price
            Carts = await _cartRepository.GetCartByUserIdAsync(int.Parse(userId));
            TotalPrice = Carts.Sum(c => c.Product.Price * c.Quantity);

            return RedirectToPage();  // Reload the page to reflect the updated cart
        }

        // Action to delete an item from the cart
        public async Task<IActionResult> OnPostDeleteItemAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the cart item for the user and product
            var cartItem = await _cartRepository.GetCartItemAsync(int.Parse(userId), productId);

            if (cartItem != null)
            {
                await _cartRepository.Delete(cartItem.CartId); // Delete the cart item from the database
            }

            // Recalculate total price
            Carts = await _cartRepository.GetCartByUserIdAsync(int.Parse(userId));
            TotalPrice = Carts.Sum(c => c.Product.Price * c.Quantity);

            return RedirectToPage();  // Reload the page to reflect the updated cart
        }

        public async Task<IActionResult> OnPostProceedToPaymentAsync(string selectedItems)
        {
            if (string.IsNullOrEmpty(selectedItems))
            {
                ModelState.AddModelError(string.Empty, "No items selected for payment.");
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Error");

            // Parse selected items to get the list of selected product IDs
            var selectedProductIds = selectedItems.Split(',')
                .Select(int.Parse)
                .ToList();

            Carts = (await _cartRepository.GetCartByUserIdAsync(int.Parse(userId)) ?? new List<Cart>())
                .Where(c => selectedProductIds.Contains(c.Product.ProductId))
                .ToList();

            // Calculate TotalPrice in VND
            TotalPrice = Carts.Sum(c => c.Product.Price * 500 * c.Quantity);

            if (TotalPrice < 1)
            {
                ModelState.AddModelError(string.Empty, "Total amount must be at least 1 VND.");
                return Page();
            }

            // Tiếp tục xử lý thanh toán như trong mã của bạn
            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var orderIds = new List<int>();

            foreach (var cartItem in Carts)
            {
                var order = new Order
                {
                    OrderNo = $"ORD{orderCode}",
                    ProductId = cartItem.Product.ProductId,
                    Quantity = cartItem.Quantity,
                    UserId = int.Parse(userId),
                    Status = "Pending",
                    PaymentId = 1,
                    OrderDate = DateTime.Now
                };

                await _orderRepository.Add(order);
                orderIds.Add(order.OrderDetailsId);
            }

            var items = Carts.Select(cartItem => new ItemData(
                cartItem.Product.Name,
                cartItem.Quantity,
                Convert.ToInt32(cartItem.Product.Price * cartItem.Quantity)
            )).ToList();

			var orderIdsString = string.Join(",", orderIds); // Ghép danh sách các order ID thành chuỗi
			var paymentData = new PaymentData(
				orderCode,
				Convert.ToInt32(TotalPrice),
				"Cart Payment",
				items,
				$"https://localhost:7234/Users/CancelPayment?orderIds={orderIdsString}", // URL hủy
				$"https://localhost:7234/Users/CompletePayment?orderIds={orderIdsString}" // URL thành công
			);

            var paymentResult = await _paymentService.CreatePaymentLinkAsync(paymentData);
            if (paymentResult != null && paymentResult.checkoutUrl != null)
            {
                return Redirect(paymentResult.checkoutUrl);
            }

            return RedirectToAction("Error");
        }
        public async Task<IActionResult> CancelPayment(string orderIds)
        {
            if (string.IsNullOrEmpty(orderIds))
            {
                ModelState.AddModelError(string.Empty, "No order IDs provided.");
                return RedirectToPage("/Users/Profile"); // Redirect nếu không có order ID nào
            }

            var orderIdList = orderIds.Split(',').Select(int.Parse).ToList();

            foreach (var orderId in orderIdList)
            {
                var order = await _orderRepository.GetOrderById(orderId);
                if (order != null && order.Status == "Pending")
                {
                    await _orderRepository.Delete(orderId); // Xóa đơn hàng có trạng thái "Pending"
                }
            }

            return RedirectToPage("/Users/Profile"); // Redirect tới trang profile hoặc trang khác
        }


        public async Task<IActionResult> CompletePayment(string orderIds)
		{
			var orderIdList = orderIds.Split(',').Select(int.Parse).ToList();

			foreach (var orderId in orderIdList)
			{
				var order = await _orderRepository.GetOrderById(orderId);
				if (order != null && order.Status == "Pending")
				{
					order.Status = "Completed";
					order.PaymentId = 1; // Example: Set to a specific ID to indicate successful payment
					await _orderRepository.Update(order);
				}
			}

			return RedirectToPage("/Users/Profile"); // Redirect to profile or another page
		}


	}
}
