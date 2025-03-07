using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Food.Repositories;
using Models;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Food.Pages.Users
{
    [Authorize(Roles ="User")]
    public class UpdateProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly string _imagePath = "/Images/User/";

        public UpdateProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public User UserProfile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserProfile = await _userRepository.GetUserById(int.Parse(userId));

            if (UserProfile == null)
            {
                return RedirectToPage("/Users/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUser = await _userRepository.GetUserById(int.Parse(userId));

            if (existingUser == null)
            {
                return RedirectToPage("/Users/Login");
            }

            existingUser.Name = UserProfile.Name;
            existingUser.Username = UserProfile.Username;
            existingUser.Email = UserProfile.Email;
            existingUser.Mobile = UserProfile.Mobile;
            existingUser.Address = UserProfile.Address;
            existingUser.PostCode = UserProfile.PostCode;

            if (Request.Form.Files.Count > 0)
            {
                // Lấy tệp tải lên
                var file = Request.Form.Files[0];

                // Lấy phần mở rộng của tệp
                var fileExtension = Path.GetExtension(file.FileName);

                // Tạo tên tệp mới từ UserId và phần mở rộng
                var fileName = $"{existingUser.UserId}{fileExtension}";

                // Đặt đường dẫn lưu tệp trong thư mục wwwroot/Images/User
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "User", fileName);

                // Lưu tệp vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn của hình ảnh vào trường ImageUrl trong cơ sở dữ liệu
                existingUser.ImageUrl = $"/Images/User/{fileName}";
            }

            // Cập nhật thông tin người dùng trong cơ sở dữ liệu
            await _userRepository.Update(existingUser);

            // Chuyển hướng đến trang Profile
            return RedirectToPage("/Users/Profile");
        }
    }
}
