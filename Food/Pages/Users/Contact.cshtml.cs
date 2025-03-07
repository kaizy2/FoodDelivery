using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Food.Repositories;
using Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Foodie.Pages.Users
{
    public class ContactModel : PageModel
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUserRepository _userRepository;

        public ContactModel(IContactRepository contactRepository, IUserRepository userRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
        }

        [BindProperty]
        public Contact Contact { get; set; }
        public string Message { get; set; }
        public string MessageClass { get; set; }
        public string SubjectError { get; set; }
        public string MessageError { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Contact = new Contact();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Users/Login");
            }

            var user = await _userRepository.GetUserById(int.Parse(userId));
            if (user != null)
            {
                Contact.Name = user.Name;
                Contact.Email = user.Email;
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            bool isValid = true;

            // Subject validation
            if (string.IsNullOrWhiteSpace(Contact.Subject) || Contact.Subject.StartsWith(" "))
            {
                SubjectError = "Subject cannot be empty or start with whitespace.";
                isValid = false;
            }

            // Message validation
            if (string.IsNullOrWhiteSpace(Contact.Message) || Contact.Message.StartsWith(" "))
            {
                MessageError = "Message cannot be empty or start with whitespace.";
                isValid = false;
            }

            if (isValid)
            {
                try
                {
                    await _contactRepository.Add(Contact);

                    Message = "Thanks for reaching out! We will look into your query.";
                    MessageClass = "alert alert-success";
                    Contact = new Contact(); // Clear form data
                }
                catch (Exception ex)
                {
                    Message = $"Error: {ex.Message}";
                    MessageClass = "alert alert-danger";
                }
            }
            else
            {
                MessageClass = "alert alert-danger";
            }

            return Page();
        }
    }
}
