using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Models;
using System.IO;
using System.Threading.Tasks;

namespace Food.Pages.Admin
{
    public class EditCategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHubContext<SignalRService> _hubContext;

        public EditCategoryModel(ICategoryRepository categoryRepository, IHubContext<SignalRService> hubContext)
        {
            _categoryRepository = categoryRepository;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryRepository.GetCategoryById(id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile CategoryImage)
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
                var existingCategory = await _categoryRepository.GetCategoryById(Category.CategoryId);
                if (existingCategory != null)
                {
                    Category.ImageUrl = existingCategory.ImageUrl;
                }
            }

            await _categoryRepository.Update(Category);

            // Gửi thông báo cập nhật tới các client qua SignalR
            var updatedCategory = new
            {
                CategoryId = Category.CategoryId,
                Name = Category.Name,
                IsActive = Category.IsActive,
                CreatedDate = Category.CreatedDate,
                ImageUrl = Category.ImageUrl
            };
            await _hubContext.Clients.All.SendAsync("ReceiveUpdatedCategory", updatedCategory);

            return RedirectToPage("Category");
        }
    }
}
