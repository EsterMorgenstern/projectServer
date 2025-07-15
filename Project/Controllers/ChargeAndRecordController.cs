using BLL.Api;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargeAndRecordController : ControllerBase
    {
        private readonly IBLLPayment paymentService;
        private readonly IPaymentGateway paymentGateway;

        public ChargeAndRecordController(IBLLPayment paymentService, IPaymentGateway paymentGateway)
        {
            this.paymentService = paymentService;
            this.paymentGateway = paymentGateway;
        }

        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                // יצירת תשלום עם סטטוס PENDING
                var payment = new BLLPayment
                {
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "CREDIT_CARD",
                    Status = "PENDING",
                    PaymentMethodId = request.PaymentMethodId,
                    GroupId = request.GroupId,
                    Notes = request.Notes,
                    CreatedAt = DateTime.Now
                };

                // ביצוע החיוב דרך Stripe
                string transactionId = await paymentGateway.ChargeAsync(
                    request.Amount,
                    "ILS",
                    request.StripeToken
                );

                // עדכון התשלום לסטטוס COMPLETED
                payment.TransactionId = transactionId;
                payment.Status = "COMPLETED";

                paymentService.Create(payment);

                return Ok(new
                {
                    success = true,
                    transactionId = transactionId,
                    message = "התשלום בוצע בהצלחה",
                    paymentId = payment.PaymentId
                });
            }
            catch (Exception ex)
            {
                // במקרה של שגיאה - יצירת רשומת תשלום נכשל
                var failedPayment = new BLLPayment
                {
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "CREDIT_CARD",
                    Status = "FAILED",
                    PaymentMethodId = request.PaymentMethodId,
                    GroupId = request.GroupId,
                    Notes = $"שגיאה: {ex.Message}",
                    CreatedAt = DateTime.Now
                };

                paymentService.Create(failedPayment);

                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    error = "התשלום נכשל"
                });
            }
        }
    }

    public class PaymentRequest
    {
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public string? StripeToken { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? GroupId { get; set; }
        public string? Notes { get; set; }
    }
}
