using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonCancellationsController : ControllerBase
    {
        IBLLLessonCancellations lessonCancellations;

        public LessonCancellationsController(IBLL manager)
        {
            lessonCancellations = manager.LessonCancellations;
        }

        // הפונקציות הקיימות שלך...
        [HttpGet("GetAll")]
        public List<BLLLessonCancellations> Get()
        {
            return lessonCancellations.Get();
        }

        [HttpGet("GetById/{id}")]
        public BLLLessonCancellations GetById(int id)
        {
            return lessonCancellations.GetById(id);
        }

        [HttpPost("Add")]
        public void Create([FromBody] BLLLessonCancellations lc)
        {
            lessonCancellations.Create(lc);
        }

        [HttpPut("Update")]
        public void Update([FromBody] BLLLessonCancellations lc)
        {
            lessonCancellations.Update(lc);
        }

        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            lessonCancellations.Delete(id);
        }


        // ביטול שיעורים לכל הקבוצות ביום מסוים
        [HttpPost("CancelAllGroupsForDay")]
        public IActionResult CancelAllGroupsForDay([FromBody] CancelAllGroupsRequest request)
        {
            try
            {
                lessonCancellations.CancelAllGroupsForDay(
                    request.DayOfWeek,
                    request.Date,
                    request.Reason,
                    request.CreatedBy
                );
                return Ok(new { message = "כל השיעורים בוטלו בהצלחה" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // קבלת כל הביטולים לתאריך מסוים
        [HttpGet("GetCancellationsByDate")]
        public List<BLLLessonCancellations> GetCancellationsByDate([FromQuery] DateTime date)
        {
            return lessonCancellations.GetCancellationsByDate(date);
        }

        // קבלת פרטי ביטולים מורחבים לתאריך מסוים
        [HttpGet("GetCancellationDetailsByDate")]
        public List<BLLLessonCancellationsDetails> GetCancellationDetailsByDate([FromQuery] DateTime date)
        {
            return lessonCancellations.GetCancellationDetailsByDate(date);
        }

        // ביטול כל הביטולים ליום מסוים (החזרת השיעורים)
        [HttpDelete("RemoveAllCancellationsForDay")]
        public IActionResult RemoveAllCancellationsForDay([FromQuery] string dayOfWeek, [FromQuery] DateTime date)
        {
            try
            {
                lessonCancellations.RemoveAllCancellationsForDay(dayOfWeek, date);
                return Ok(new { message = "כל הביטולים הוסרו בהצלחה" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // קבלת קבוצות זמינות לביטול ביום מסוים
        [HttpGet("GetGroupsAvailableForCancellation")]
        public List<BLLGroupDetails> GetGroupsAvailableForCancellation([FromQuery] string dayOfWeek, [FromQuery] DateTime date)
        {
            return lessonCancellations.GetGroupsAvailableForCancellation(dayOfWeek, date);
        }

        // בדיקה האם יש ביטולים ביום מסוים
        [HttpGet("HasCancellationsForDay")]
        public IActionResult HasCancellationsForDay([FromQuery] string dayOfWeek, [FromQuery] DateTime date)
        {
            try
            {
                var cancellations = lessonCancellations.GetCancellationsByDate(date);
                var hasCancellations = cancellations.Any();

                return Ok(new
                {
                    hasCancellations = hasCancellations,
                    cancellationsCount = cancellations.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // **מודלים לבקשות:**
    public class CancelAllGroupsRequest
    {
        public string? DayOfWeek { get; set; }
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
