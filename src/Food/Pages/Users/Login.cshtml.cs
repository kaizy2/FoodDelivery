using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Food.Repositories;
using Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;

    public LoginModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [TempData]
    public string Message { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Kiểm tra đăng nhập
        var user = await _userRepository.Login(Username, Password);
        if (user != null)
        {
            // Kiểm tra nếu người dùng đã được xác thực
            if (!user.IsEmailVerified)
            {
                Message = "Your email is not verified yet. Please verify your email to log in.";
                return Page(); // Trả về trang đăng nhập nếu email chưa xác thực
            }

            // Tạo các Claims cho người dùng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role), 
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Thêm ID người dùng vào claim
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Đăng nhập người dùng với Claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

           
            if (user.Role == "Admin")
            {
                
                return RedirectToPage("/Admin/Dashboard");
            }
            else
            {
                
                return RedirectToPage("/Users/Default");
            }
        }

        // Nếu đăng nhập thất bại
        Message = "Invalid username or password";
        return Page();
    }
}
