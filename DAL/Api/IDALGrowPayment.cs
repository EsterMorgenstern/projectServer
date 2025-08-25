using DAL.Models;

namespace DAL.Api
{
    public interface IDALGrowPayment
    {
        Task<GrowPaymentResponse> CreatePaymentProcessAsync(GrowPaymentRequest req);
    }
}
