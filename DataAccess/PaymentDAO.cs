using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PaymentDAO : SingletonBase<PaymentDAO>
    {
        public async Task<IEnumerable<Payment>> GetPaymentAll()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
            return payment;
        }

        public async Task Add(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Payment payment)
        {
            var existingItem = await GetPaymentById(payment.PaymentId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(payment);
            }
            else
            {
                _context.Payments.Add(payment);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var payment = await GetPaymentById(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
