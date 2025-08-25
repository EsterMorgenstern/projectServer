using BLL.Api;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        IBLLPayment payment;

        public PaymentsController(IBLL manager)
        {
            payment = manager.Payments;
        }

        [HttpGet("GetAll")]
        public List<BLLPayment> Get()
        {
            return payment.Get();
        }

        [HttpGet("GetByStudentId/{studentId}")]
        public List<BLLPayment> GetByStudentId(int studentId)
        {
            return payment.GetByStudentId(studentId);
        }

        [HttpGet("GetById/{paymentId}")]
        public BLLPayment GetById(int paymentId)
        {
            return payment.GetById(paymentId);
        }

        [HttpGet("GetByPaymentMethodId/{paymentMethodId}")]
        public List<BLLPayment> GetByPaymentMethodId(int paymentMethodId)
        {
            return payment.GetByPaymentMethodId(paymentMethodId);
        }

        [HttpPost("Add")]
        public IActionResult Create([FromBody] BLLPayment paymentData)
        {
            try
            {
                paymentData.CreatedAt = DateTime.Now;
                if (paymentData.PaymentDate == default(DateTime))
                {
                    paymentData.PaymentDate = DateTime.Now;
                }
                if (string.IsNullOrEmpty(paymentData.Status))
                {
                    paymentData.Status = "COMPLETED";
                }
                if (string.IsNullOrEmpty(paymentData.TransactionId))
                {
                    paymentData.TransactionId = Guid.NewGuid().ToString();
                }

                payment.Create(paymentData);
                return Ok(new { message = "Payment created successfully", transactionId = paymentData.TransactionId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] BLLPayment paymentData)
        {
            try
            {
                payment.Update(paymentData);
                return Ok(new { message = "Payment updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("Delete/{paymentId}")]
        public IActionResult Delete(int paymentId)
        {
            try
            {
                payment.Delete(paymentId);
                return Ok(new { message = "Payment deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("CreateGrowWalletPayment")]
        public async Task<IActionResult> CreateGrowWalletPayment([FromForm] GrowPaymentRequest request)
        {
            try
            {
                // קריאה לפונקציה ב-BLL
                string redirectUrl = await this.payment.CreateGrowWalletPaymentAsync(
                    new BLLPayment
                    {
                        StudentId = request.studentId,
                        Amount = request.sum,
                        PaymentMethod = "Grow Wallet",
                        Notes = request.description,
                        PaymentDate = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        Status = "PENDING"
                    },
                    request.sum,
                    request.pageField_fullName,
                    request.pageField_phone,
                    request.description,
                    request.studentId,
                    request.creditCardNumber // העברת מספר האשראי
                );

                if (string.IsNullOrEmpty(redirectUrl))
                {
                    return BadRequest(new { error = "Failed to create payment process. RedirectUrl is null." });
                }

                return Ok(new { message = "Payment process created successfully", redirectUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("PaymentCallback")]
        public async Task<IActionResult> PaymentCallback([FromBody] GrowPaymentResponse response)
        {
            try
            {
                if (response.status == 1) // סטטוס 1 מציין הצלחה
                {
                    var paymentToUpdate = this.payment.GetByTransactionId(response.data.paymentCode);
                    if (paymentToUpdate != null)
                    {
                        paymentToUpdate.Status = "COMPLETED";
                        this.payment.Update(paymentToUpdate);

                        // אישור התשלום ל-Grow Wallet
                        await this.payment.ApproveTransactionAsync(response.data.paymentCode);
                    }
                }
                else
                {
                    var paymentToUpdate = this.payment.GetByTransactionId(response.data.paymentCode);
                    if (paymentToUpdate != null)
                    {
                        paymentToUpdate.Status = "FAILED";
                        this.payment.Update(paymentToUpdate);
                    }
                }

                return Ok(new { message = "Callback processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        
    }
}

