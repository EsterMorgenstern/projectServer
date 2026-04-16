using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentHealthFundController : ControllerBase
    {
        private readonly IBLLStudentHealthFund studentHealthFunds;

        public StudentHealthFundController(IBLL manager)
        {
            studentHealthFunds = manager.StudentHealthFunds;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<BLLStudentHealthFundPerfect>> Get()
        {
            try
            {
                var result = studentHealthFunds.Get();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת נתוני הגביה: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public ActionResult<BLLStudentHealthFund> GetById(int id)
        {
            try
            {
                var result = studentHealthFunds.GetById(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת רשומת גביה: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Create([FromBody] BLLStudentHealthFund studentHealthFund)
        {
            try
            {
                await studentHealthFunds.Create(studentHealthFund);
                return Ok(new { message = "רשומת גביה נוצרה בהצלחה" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה ביצירת רשומת גביה: {ex.Message}");
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] BLLStudentHealthFund studentHealthFund)
        {
            try
            {
                if (id != studentHealthFund.Id)
                {
                    return BadRequest("חוסר התאמה בין מזהה הרשומה לנתונים שנשלחו");
                }

                studentHealthFunds.Update(studentHealthFund);
                return Ok(new { message = "רשומת גביה עודכנה בהצלחה" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בעדכון רשומת גביה: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{studentHealthFundId}")]
        public IActionResult Delete(int studentHealthFundId)
        {
            try
            {
                studentHealthFunds.Delete(studentHealthFundId);
                return Ok(new { message = "רשומת גביה נמחקה בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה במחיקת רשומת גביה: {ex.Message}");
            }
        }

        [HttpGet("{id}/reported-dates")]
        public ActionResult<List<DateTime>> GetReportedDates(int id)
        {
            try
            {
                var result = studentHealthFunds.GetReportedDates(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת תאריכים מדווחים: {ex.Message}");
            }
        }

        [HttpGet("{id}/unreported-dates")]
        public ActionResult<List<DateTime>> GetUnreportedDates(int id)
        {
            try
            {
                var result = studentHealthFunds.GetUnreportedDates(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בקבלת תאריכים לא מדווחים: {ex.Message}");
            }
        }

        [HttpPost("{id}/ReportUnreportedDate")]
        public async Task<IActionResult> ReportUnreportedDate(int id, [FromBody] DateTime date)
        {
            try
            {
                await studentHealthFunds.ReportUnreportedDate(id, date);
                return Ok(new { message = "התאריך דווח בהצלחה" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בדיווח התאריך: {ex.Message}");
            }
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, int studentHealthFundId, string fileType)
        {
            try
            {
                if (file == null)
                    return BadRequest("לא התקבל קובץ");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", studentHealthFundId.ToString());
                Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                studentHealthFunds.UploadFile(studentHealthFundId, filePath, fileType);

                return Ok(new { filePath, message = "הקובץ הועלה בהצלחה" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בהעלאת הקובץ: {ex.Message}");
            }
        }

        [HttpPost("ValidateAndFixUnreportedTreatments")]
        public async Task<IActionResult> SyncUnreportedTreatments()
        {
            try
            {
                var result = await studentHealthFunds.ValidateAndFixUnreportedTreatments();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בסנכרון הנתונים: {ex.Message}");
            }
        }
    }
}