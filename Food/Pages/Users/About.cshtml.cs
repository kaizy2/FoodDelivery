using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Food.Pages.Users
{
    public class AboutModel : PageModel
    {
        private readonly FoodDbContext _context;

        public AboutModel(FoodDbContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch active categories from the database
            Categories = await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}
