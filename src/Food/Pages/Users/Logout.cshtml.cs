using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Food.Pages.Users
{
    [AllowAnonymous] // Make sure that this page is accessible without login
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(); // Sign the user out of the application
            TempData["Message"] = "You have successfully logged out.";
            // Redirect the user to the login page
            return RedirectToPage("/Users/Login");
        }
    }
}
