using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Food.Repositories;
using Models;
using System.Threading.Tasks;

namespace Food.Pages.Users
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string code)
        {
            var user = await _userRepository.GetUserByEmail(email);

            // Kiểm tra mã xác thực
            if (user == null || user.VerificationCode != code)
            {
                Message = "Invalid or expired verification code.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string email, string code)
        {
            if (NewPassword != ConfirmPassword)
            {
                Message = "Passwords do not match.";
                return Page();
            }

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || user.VerificationCode != code)
            {
                Message = "Invalid or expired verification code.";
                return Page();
            }

            // Cập nhật mật khẩu người dùng
            user.Password = NewPassword;
            user.VerificationCode = null; // Reset verification code

            await _userRepository.Update(user);

            Message = "Password successfully updated.";
            return RedirectToPage("/Users/Login");
        }
    }
}
