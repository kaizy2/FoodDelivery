using Food.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Food.Pages.Admin.Product
{
    public class EditProductModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public EditProductModel(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Models.Product Product { get; set; }
        public List<Category> CategoriesList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productRepository.GetProductById(id);
            if (Product == null)
            {
                return NotFound();
            }

            CategoriesList = (await _categoryRepository.GetAllCategories()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile ProductImage)
        {
            if (ProductImage != null)
            {
                var filePath = Path.Combine("wwwroot/Images/Products", ProductImage.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                Product.ImageUrl = $"/Images/Products/{ProductImage.FileName}";
            }
            else
            {
                var existingProduct = await _productRepository.GetProductById(Product.ProductId);
                if (existingProduct != null)
                {
                    Product.ImageUrl = existingProduct.ImageUrl;
                }
            }

            await _productRepository.Update(Product);
            return RedirectToPage("ViewProduct");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _productRepository.Update(Product);
            return RedirectToPage("ViewProduct"); // Chuyển hướng về trang ViewProduct sau khi cập nhật
        }
    }
}
