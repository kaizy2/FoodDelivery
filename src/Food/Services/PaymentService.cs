using Net.payOS;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;


namespace Food.Services
{
    public class PaymentService
    {
        private readonly PayOS _payOS;

        public PaymentService(IConfiguration configuration)
        {
            _payOS = new PayOS(
                configuration["PayOS:ClientId"],
                configuration["PayOS:ApiKey"],
                configuration["PayOS:ChecksumKey"]
            );
        }

        public async Task<CreatePaymentResult> CreatePaymentLinkAsync(PaymentData paymentData)
        {
            return await _payOS.createPaymentLink(paymentData);
        }
    }
}
