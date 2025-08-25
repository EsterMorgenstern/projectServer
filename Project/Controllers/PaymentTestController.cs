using BLL.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentTestController : ControllerBase
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IConfiguration _configuration;

    public PaymentTestController(IPaymentGateway paymentGateway, IConfiguration configuration)
    {
        _paymentGateway = paymentGateway;
        _configuration = configuration;
    }

    // ✅ בדיקת הגדרת Stripe
    [HttpGet("stripe-status")]
    public IActionResult GetStripeStatus()
    {
        try
        {
            var secretKey = _configuration["Stripe:SecretKey"];
            var publishableKey = _configuration["Stripe:PublishableKey"];

            return Ok(new
            {
                IsConfigured = !string.IsNullOrEmpty(secretKey),
                SecretKeyPrefix = secretKey?.Substring(0, 12) + "...",
                PublishableKey = publishableKey,
                Environment = secretKey?.Contains("test") == true ? "Test" : "Live"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    //// ✅ בדיקת תשלום פשוט
    //[HttpPost("test-payment")]
    //public async Task<IActionResult> TestPayment([FromBody] TestPaymentRequest request)
    //{
    //    try
    //    {
    //        Console.WriteLine($"🧪 Testing payment: {request.Amount} for student {request.StudentId}");

    //        var result = await _paymentGateway.ProcessPaymentAsync(new PaymentRequest
    //        {
    //            Amount = request.Amount,
    //            Currency = "ILS",
    //            PaymentMethodId = request.PaymentMethodId,
    //            StudentId = request.StudentId,
    //            Description = $"Test payment for student {request.StudentId}",
    //            Metadata = new Dictionary<string, string>
    //            {
    //                ["test"] = "true",
    //                ["student_id"] = request.StudentId.ToString(),
    //                ["timestamp"] = DateTime.UtcNow.ToString()
    //            }
    //        });

    //        Console.WriteLine($"✅ Payment result: {result.IsSuccess}");
    //        Console.WriteLine($"📄 Transaction ID: {result.TransactionId}");

    //        return Ok(new
    //        {
    //            Success = result.IsSuccess,
    //            TransactionId = result.TransactionId,
    //            Message = result.ErrorMessage ?? "Payment processed successfully",
    //            Amount = request.Amount,
    //            Currency = "ILS",
    //            TestMode = true
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"❌ Payment error: {ex.Message}");
    //        return BadRequest(new { Error = ex.Message });
    //    }
    //}

    // ✅ רשימת כרטיסי בדיקה
    [HttpGet("test-cards")]
    public IActionResult GetTestCards()
    {
        return Ok(new
        {
            TestCards = new[]
            {
                new { Number = "4242424242424242", Brand = "Visa", Description = "תשלום מוצלח" },
                new { Number = "4000000000000002", Brand = "Visa", Description = "כרטיס נדחה" },
                new { Number = "4000000000009995", Brand = "Visa", Description = "כספים לא מספיקים" },
                new { Number = "4000000000000069", Brand = "Visa", Description = "כרטיס פג תוקף" },
                new { Number = "5555555555554444", Brand = "Mastercard", Description = "תשלום מוצלח" }
            },
            Instructions = new[]
            {
                "השתמש בכל תאריך עתידי לתפוגה",
                "השתמש בכל CVC (3 ספרות)",
                "השתמש בכל מיקוד"
            }
        });
    }
}

public class TestPaymentRequest
{
    public decimal Amount { get; set; }
    public string PaymentMethodId { get; set; }
    public int StudentId { get; set; }
}
