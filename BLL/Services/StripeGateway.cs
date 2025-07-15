using Stripe;

namespace BLL.Services
{
    public class StripeGateway : IPaymentGateway
    {
        public StripeGateway(string secretKey)
        {
            StripeConfiguration.ApiKey = secretKey;
        }

        public async Task<string> ChargeAsync(decimal amount, string currency, string token)
        {
            var options = new ChargeCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe requires amount in cents
                Currency = currency,
                Source = token,
                Description = "Payment via Stripe"
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Status != "succeeded")
                throw new System.Exception("Charge failed");

            return charge.Id; // Transaction ID
        }
    }
}
