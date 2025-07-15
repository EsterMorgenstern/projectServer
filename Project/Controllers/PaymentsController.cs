using BLL.Api;
using Microsoft.AspNetCore.Mvc;

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
    }
}

