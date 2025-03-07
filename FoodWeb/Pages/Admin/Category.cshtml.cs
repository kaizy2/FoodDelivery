using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace FoodWeb.Pages.Admin
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [BindProperty]
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public void OnGetAsync()
        {
            Categories = (await _categoryRepository.GetAllCategories()).ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _categoryRepository.Add(Category);
            return RedirectToPage();
        }
    }
}
