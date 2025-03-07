using Food.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Food.Pages.Admin
{
	[Authorize(Roles = "Admin")]
	public class DashboardModel : PageModel
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IUserRepository _userRepository;

		public int TotalOrders { get; set; }
		public int TotalUsers { get; set; }

		public DashboardModel(IOrderRepository orderRepository, IUserRepository userRepository)
		{
			_orderRepository = orderRepository;
			_userRepository = userRepository;
		}

		public async Task OnGetAsync()
		{
			// Lấy tổng số lượng orders
			TotalOrders = await _orderRepository.GetOrderCount();

			// Lấy tổng số lượng users
			TotalUsers = await _userRepository.GetUserCount();
		}
	}
}
