using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthFundCommitmentController : ControllerBase
    {
        private readonly IDAL dal;

        public HealthFundCommitmentController(IDAL dal)
        {
            this.dal = dal;
        }

        [HttpGet("ByStudentHealthFund/{studentHealthFundId}")]
        public IActionResult GetByStudentHealthFundId(int studentHealthFundId)
        {
            try
            {
                var result = dal.HealthFundCommitments.GetByStudentHealthFundId(studentHealthFundId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת התחייבויות: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = dal.HealthFundCommitments.GetById(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת התחייבות: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Create([FromBody] HealthFundCommitment commitment)
        {
            try
            {
                await dal.HealthFundCommitments.Create(commitment);
                return Ok(new { message = "התחייבות נוספה בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה ביצירת התחייבות: {ex.Message}");
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HealthFundCommitment commitment)
        {
            try
            {
                if (id != commitment.Id)
                {
                    return BadRequest("חוסר התאמה בין המזהה לנתונים");
                }

                await dal.HealthFundCommitments.Update(commitment);
                return Ok(new { message = "התחייבות עודכנה בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בעדכון התחייבות: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await dal.HealthFundCommitments.Delete(id);
                return Ok(new { message = "התחייבות נמחקה בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה במחיקת התחייבות: {ex.Message}");
            }
        }
    }
}