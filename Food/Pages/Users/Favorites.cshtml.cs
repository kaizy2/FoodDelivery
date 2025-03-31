using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Food.Repositories;
using Models;
using System.Threading.Tasks;

namespace Food.Pages.Users
{
    public class FavoritesModel : PageModel
    {
        public void OnGet()
        {
            // Không cần xử lý logic vì dữ liệu lấy từ LocalStorage (client-side)
        }
    }
}
