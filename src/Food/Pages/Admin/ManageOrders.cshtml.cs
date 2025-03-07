using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace Food.Pages.Admin
{
	[Authorize(Roles = "Admin")]
	public class ManageOrdersModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private const int PageSize = 6; // Số lượng đơn hàng trên mỗi trang

        public ManageOrdersModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<Order> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            var allOrders = await _orderRepository.GetAllOrders();
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(allOrders.Count() / (double)PageSize);

            Orders = allOrders
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();


        }

        public async Task<IActionResult> OnPostDeleteOrderAsync(int id, int page = 1)
        {
            await _orderRepository.Delete(id);
            // Điều chỉnh để quay lại trang hiện tại sau khi xóa
            return RedirectToPage("/Admin/ManageOrders", new { page });
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string newStatus, int page = 1)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order != null && order.Status != newStatus)
            {
                order.Status = newStatus;
                await _orderRepository.Update(order);
            }

            // Điều chỉnh để quay lại trang hiện tại sau khi cập nhật
            return RedirectToPage("/Admin/ManageOrders", new { page });
        }
    }
}
