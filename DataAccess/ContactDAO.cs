using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ContactDAO : SingletonBase<ContactDAO>
    {
        public async Task<IEnumerable<Contact>> GetContactAll()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> GetContactById(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.ContactId == id);
            return contact;
        }

        public async Task Add(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Contact contact)
        {
            var existingItem = await GetContactById(contact.ContactId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(contact);
            }
            else
            {
                _context.Contacts.Add(contact);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var contact = await GetContactById(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }
}
