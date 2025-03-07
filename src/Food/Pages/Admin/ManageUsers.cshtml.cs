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
	public class ManageUsersModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private const int PageSize = 6;

        public ManageUsersModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            var allUsers = await _userRepository.GetUsersWithUserRole();
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(allUsers.Count() / (double)PageSize);

            Users = allUsers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int id, int page = 1)
        {
            await _userRepository.Delete(id);
            return RedirectToPage(new { page }); // Quay lại trang hiện tại sau khi xóa
        }
    }
}
