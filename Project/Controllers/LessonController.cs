using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LessonController : ControllerBase
    {
        IBLLLesson lessons;
        public LessonController(IBLL manager)
        {
            lessons = manager.Lessons;
        }
        [HttpPost("Add")]
        public void GenerateLessonsForGroup(BLLGroup group)
        {
            lessons.GenerateLessonsForGroup(group.GroupId, (DateOnly)group.StartDate, (int)group.NumOfLessons, group.DayOfWeek, (TimeOnly)group.Hour, group.InstructorId, "system");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteLesson(int id)
        {
            try
            {
                lessons.Delete(id);
                return Ok(new { message = "Lesson deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ByDate")]
        public ActionResult<List<LessonCalendarItemDto>> GetLessonsByDate([FromQuery] DateOnly date)
        {
            try
            {
                var result = lessons.GetLessonsForCalendarByDate(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByDateRange")]
        public ActionResult<List<LessonCalendarItemDto>> GetLessonsByDateRange(
         [FromQuery] DateOnly startDate,
         [FromQuery] DateOnly endDate)
        {
            try
            {
                var result = lessons.GetLessonsForCalendarByDateRange(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cancel/{id}")]
        public IActionResult CancelLesson(int id, string reason, string canceledBy)
        {
            try
            {
                lessons.CancelLesson(id, reason, canceledBy);
                return Ok(new { message = "Lesson canceled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("cancel-all-for-day")]
        public IActionResult CancelAllForDay(string dayOfWeek, DateOnly date, string reason, string createdBy)
        {
            try
            {
                lessons.CancelAllGroupsForDay(dayOfWeek, date, reason, createdBy);
                return Ok(new { message = "All lessons for day canceled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("canceled/{date}")]
        public IActionResult GetCanceledByDate(string date)
        {
            try
            {
                var dateOnly = DateOnly.Parse(date);
                var canceled = lessons.GetCanceledLessonsByDate(dateOnly);
                return Ok(canceled);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("undo-cancel/{id}")]
        public IActionResult UndoCancelLesson(int id, string undoBy)
        {
            try
            {
                lessons.UndoCancelLesson(id, undoBy);
                return Ok(new { message = "Lesson cancellation undone successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("canceled")]
        public IActionResult GetCanceledLessons()
        {
            try
            {
                var canceledLessons = lessons.GetCanceledLessons();
                return Ok(canceledLessons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("completion/create")]
        public async Task<IActionResult> CreateCompletionLesson(int groupId, DateOnly completionDate,
            TimeOnly completionHour, int instructorId, string createdBy)
        {
            try
            {
                await lessons.CreateCompletionLesson(groupId, completionDate,
                    completionHour, instructorId, createdBy);
                return Ok(new { message = "Completion lesson created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("mark-as-completion/{id}")]
        public IActionResult MarkLessonAsCompletion(int id, string markedBy)
        {
            try
            {
                lessons.MarkLessonAsCompletion(id, markedBy);
                return Ok(new { message = "Lesson marked as completion successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("completion")]
        public IActionResult GetCompletionLessons()
        {
            try
            {
                var completionLessons = lessons.GetCompletionLessons();
                return Ok(completionLessons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("completion/by-group/{groupId}")]
        public IActionResult GetCompletionLessonsByGroup(int groupId)
        {
            try
            {
                var completionLessons = lessons.GetCompletionLessonsByGroupId(groupId);
                return Ok(completionLessons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

          }
}

