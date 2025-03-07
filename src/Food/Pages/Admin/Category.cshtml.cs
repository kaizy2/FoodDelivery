using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Food.Pages.Admin
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<CategoryViewModel> CategoriesList { get; set; }

        public async Task OnGetAsync()
        {
            var categories = await _categoryRepository.GetAllCategories();
            CategoriesList = categories.Select(c => new CategoryViewModel
            {
                CategoryID = c.CategoryId,
                CategoryName = c.Name,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _categoryRepository.Delete(id);
            return RedirectToPage();
        }
    }

    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

