using Microsoft.AspNetCore.Mvc;
using Food.Repositories;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Food.Pages.Admin
{
	[Authorize(Roles = "Admin")]
	public class ContactsModel : PageModel
    {
        private readonly IContactRepository _contactRepository;

        public ContactsModel(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public List<Contact> Contacts { get; set; }

        public async Task OnGetAsync()
        {
            // Retrieve all contact messages
            Contacts = (await _contactRepository.GetAllContacts()).ToList();
        }

        public async Task<IActionResult> OnGetDeleteContactAsync(int contactId)
        {
            try
            {
                // Delete the contact message by ContactId
                await _contactRepository.Delete(contactId);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Handle any errors
                TempData["ErrorMessage"] = "Error deleting contact: " + ex.Message;
                return RedirectToPage();
            }
        }
    }
}
