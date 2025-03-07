using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Models;
using System.IO;
using System.Threading.Tasks;

namespace Food.Pages.Admin
{
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHubContext<SignalRService> _hubContext;

        public CreateCategoryModel(ICategoryRepository categoryRepository, IHubContext<SignalRService> hubContext)
        {
            _categoryRepository = categoryRepository;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile CategoryImage)
        {
            if (CategoryImage != null)
            {
                var filePath = Path.Combine("wwwroot/Images/Categories", CategoryImage.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CategoryImage.CopyToAsync(stream);
                }

                Category.ImageUrl = $"/Images/Categories/{CategoryImage.FileName}";
            }
            else
            {
                Category.ImageUrl = "/Images/Categories/default.png";
            }

            await _categoryRepository.Add(Category);

            // Gửi dữ liệu danh mục mới tới các client qua SignalR
            var newCategory = new
            {
                CategoryId = Category.CategoryId,
                Name = Category.Name,
                IsActive = Category.IsActive,
                CreatedDate = Category.CreatedDate,
                ImageUrl = Category.ImageUrl
            };
            await _hubContext.Clients.All.SendAsync("ReceiveNewCategory", newCategory);

            return RedirectToPage("Category");
        }
    }
}
