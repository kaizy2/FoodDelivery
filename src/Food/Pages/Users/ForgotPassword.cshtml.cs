using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Food.Repositories;
using Models;
using System.Threading.Tasks;

namespace Food.Pages.Users
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra email có tồn tại trong cơ sở dữ liệu
            var user = await _userRepository.GetUserByEmail(Email);
            if (user == null)
            {
                Message = "Email không tồn tại trong hệ thống.";
                return Page();
            }

            // Tạo mã xác thực ngẫu nhiên cho người dùng
            var resetCode = Guid.NewGuid().ToString();
            user.VerificationCode = resetCode;  // Lưu mã xác thực vào cơ sở dữ liệu

            await _userRepository.Update(user); // Lưu lại mã xác thực

            // Tạo liên kết reset mật khẩu
            var resetLink = Url.Page("/Users/ChangePassword",
                pageHandler: null,
                values: new { email = user.Email, code = resetCode },
                protocol: Request.Scheme);

            // Gửi email với liên kết đổi mật khẩu
            await _emailService.SendEmailAsync(user.Email, "Password Reset",
                $"Click this link to reset your password: <a href='{resetLink}'>Reset Password</a>");

            Message = "Một liên kết reset mật khẩu đã được gửi đến email của bạn.";
            return Page();
        }
    }
}
