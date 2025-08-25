using BLL.Api;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        IBLLPaymentMethod paymentMethod;

        public PaymentMethodsController(IBLL manager)
        {
            paymentMethod = manager.PaymentMethods;
        }

        [HttpGet("GetAll")]
        public List<BLLPaymentMethod> Get()
        {
            return paymentMethod.Get();
        }

        [HttpGet("GetByStudentId/{studentId}")]
        public List<BLLPaymentMethod> GetByStudentId(int studentId)
        {
            return paymentMethod.GetByStudentId(studentId);
        }

        [HttpGet("GetById/{paymentMethodId}")]
        public BLLPaymentMethod GetById(int paymentMethodId)
        {
            return paymentMethod.GetById(paymentMethodId);
        }

        [HttpPost("Add")]
        public IActionResult Create([FromBody] BLLPaymentMethod paymentMethodData)
        {
            try
            {
                paymentMethodData.CreatedAt = DateTime.Now;
                paymentMethodData.UpdatedAt = DateTime.Now;
                paymentMethodData.IsActive = true;

                paymentMethod.Create(paymentMethodData);
                return Ok(new { message = "Payment method created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] BLLPaymentMethod paymentMethodData)
        {
            try
            {
                paymentMethodData.UpdatedAt = DateTime.Now;
                paymentMethod.Update(paymentMethodData);
                return Ok(new { message = "Payment method updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("Delete/{paymentMethodId}")]
        public IActionResult Delete(int paymentMethodId)
        {
            try
            {
                paymentMethod.Delete(paymentMethodId);
                return Ok(new { message = "Payment method deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("SetAsDefault/{paymentMethodId}/{studentId}")]
        public IActionResult SetAsDefault(int paymentMethodId, int studentId)
        {
            try
            {
                paymentMethod.SetAsDefault(paymentMethodId, studentId);
                return Ok(new { message = "Default payment method updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("SaveGrowWalletPaymentMethod")]
        public IActionResult SaveGrowWalletPaymentMethod([FromBody] BLLPaymentMethod paymentMethodData)
        {
            try
            {
                paymentMethodData.CreatedAt = DateTime.Now;
                paymentMethodData.UpdatedAt = DateTime.Now;
                paymentMethodData.IsActive = true;

                paymentMethod.SaveGrowWalletPaymentMethod(paymentMethodData);
                return Ok(new { message = "Grow Wallet payment method saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
