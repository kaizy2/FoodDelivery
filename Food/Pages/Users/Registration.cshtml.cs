using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Food.Repositories;
using System.Threading.Tasks;
using System;

namespace Food.Pages.Users
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RegistrationModel(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [BindProperty]
        public User NewUser { get; set; } = new User();

        public string Message { get; private set; }
        public async Task<IActionResult> OnPostRegisterAsync()
        {

            var existingUser = await _userRepository.GetUserByEmail(NewUser.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("NewUser.Email", "This email is already in use. Please use a different email.");
                return Page();
            }
            NewUser.VerificationCode = Guid.NewGuid().ToString();

            try
            {
                NewUser.Role = "User";
                await _userRepository.Add(NewUser);
                var confirmationLink = Url.Page("/Users/ConfirmEmail",
                                   pageHandler: null,
                                   values: new { email = NewUser.Email, code = NewUser.VerificationCode },
                                   protocol: Request.Scheme);
                await _emailService.SendEmailAsync(NewUser.Email, "Email Confirmation",
                                 $"Please confirm your email by clicking on this link: <a href='{confirmationLink}'>link</a>");
                Message = "Registration successful! Please check your email to confirm your account.";
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return Page();
            }
        }
    }
}
