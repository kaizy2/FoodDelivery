using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.Repositories
{
    public class ContactRepository : IContactRepository
    {
        public async Task Add(Contact contact)
        {
            await ContactDAO.Instance.Add(contact);
        }

        public async Task Delete(int id)
        {
            await ContactDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await ContactDAO.Instance.GetContactAll();
        }

        public async Task<Contact> GetContactById(int id)
        {
            return await ContactDAO.Instance.GetContactById(id);
        }

        public async Task Update(Contact contact)
        {
            await ContactDAO.Instance.Update(contact);
        }
    }
}
