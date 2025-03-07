using Food.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Food.Pages.Admin.Product
{
    public class CreateProductModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductModel(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Models.Product Product { get; set; }
        public List<Category> CategoriesList { get; set; }

        public async Task OnGetAsync()
        {
            CategoriesList = (await _categoryRepository.GetAllCategories()).ToList();
        }



        public async Task<IActionResult> OnPostAsync(IFormFile ProductImage)
        {
            if (ProductImage != null)
            {
                // Lưu tệp hình ảnh vào thư mục `wwwroot/images/products`
                var filePath = Path.Combine("wwwroot/Images/Products", ProductImage.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn cho `ImageUrl`
                Product.ImageUrl = $"/Images/Products/{ProductImage.FileName}";
            }
            else
            {
                // Nếu không có tệp tải lên, đặt một đường dẫn mặc định cho `ImageUrl`
                Product.ImageUrl = "/Images/Products/default.png";
            }


            await _productRepository.Add(Product);
            return RedirectToPage("ViewProduct");
        }
    }
}
