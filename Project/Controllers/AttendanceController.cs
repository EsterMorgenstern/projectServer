using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        IBLLAttendance attendances;

        public AttendanceController(IBLL manager)
        {
            attendances = manager.Attendances;
        }

        [HttpGet("GetAll")]
        public List<BLLAttendance> Get()
        {
            return attendances.Get();
        }

        [HttpGet("getById/{id}")]
        public BLLAttendance GetById(int id)
        {
            return attendances.GetById(id);
        }

        [HttpPost("Add")]
        public IActionResult Create(BLLAttendance attendance)
        {
            try
            {
                attendances.Create(attendance);
                return Ok("נוכחות נוספה בהצלחה");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בהוספת נוכחות: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public IActionResult Update(BLLAttendance attendance)
        {
            try
            {
                attendances.Update(attendance);
                return Ok("נוכחות עודכנה בהצלחה");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בעדכון נוכחות: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int attendanceId)
        {
            try
            {
                attendances.Delete(attendanceId);
                return Ok("נוכחות נמחקה בהצלחה");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה במחיקת נוכחות: {ex.Message}");
            }
        }


        [HttpDelete("DeleteAttendanceByGroupAndDate/{groupId}/{date}")]
        public IActionResult DeleteAttendanceByGroupAndDate(int groupId, string date)
        {
            try
            {
                if (DateOnly.TryParse(date, out DateOnly parsedDate))
                {
                    attendances.DeleteByGroupAndDate(groupId, parsedDate);
                    return Ok("נוכחות נמחקה בהצלחה");
                }
                return BadRequest("תאריך לא תקין");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה במחיקת נוכחות: {ex.Message}");
            }
        }


        [HttpPost("SaveAttendanceForDate")]
        public IActionResult SaveAttendanceForDate([FromBody] SaveAttendanceRequest request)
        {
            try
            {
                var result = attendances.SaveAttendanceForDate(
                    request.GroupId,
                    request.Date,
                    request.AttendanceRecords
                );

                return result ? Ok("נוכחות נשמרה בהצלחה") : BadRequest("שגיאה בשמירת הנוכחות");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בשמירת נוכחות: {ex.Message}");
            }
        }

        [HttpGet("GetAttendanceByGroupAndDate/{groupId}/{date}")]
        public IActionResult GetAttendanceByGroupAndDate(int groupId, string date)
        {
            try
            {
                if (DateOnly.TryParse(date, out DateOnly parsedDate))
                {
                    var attendance = attendances.GetAttendanceByGroupAndDate(groupId, parsedDate);
                    return Ok(attendance);
                }
                return BadRequest("תאריך לא תקין");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת נוכחות: {ex.Message}");
            }
        }

        [HttpGet("GetAttendanceByGroupAndDateRange/{groupId}/{startDate}/{endDate}")]
        public IActionResult GetAttendanceByGroupAndDateRange(int groupId, string startDate, string endDate)
        {
            try
            {
                if (DateOnly.TryParse(startDate, out DateOnly start) &&
                    DateOnly.TryParse(endDate, out DateOnly end))
                {
                    var attendance = attendances.GetAttendanceByGroupAndDateRange(groupId, start, end);
                    return Ok(attendance);
                }
                return BadRequest("תאריכים לא תקינים");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת נוכחות לטווח תאריכים: {ex.Message}");
            }
        }


        [HttpGet("GetAttendanceStatistics/{groupId}")]
        public IActionResult GetAttendanceStatistics(int groupId)
        {
            try
            {
                var statistics = attendances.GetAttendanceStatistics(groupId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת סטטיסטיקות נוכחות: {ex.Message}");
            }
        }

        [HttpGet("GetAttendanceStatisticsByDateRange/{groupId}/{startDate}/{endDate}")]
        public IActionResult GetAttendanceStatisticsByDateRange(int groupId, string startDate, string endDate)
        {
            try
            {
                if (DateOnly.TryParse(startDate, out DateOnly start) &&
                    DateOnly.TryParse(endDate, out DateOnly end))
                {
                    var statistics = attendances.GetAttendanceByGroupAndDateRange(groupId, start, end);
                    return Ok(statistics);
                }
                return BadRequest("תאריכים לא תקינים");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת סטטיסטיקות נוכחות לטווח תאריכים: {ex.Message}");
            }
        }


        [HttpGet("GetAttendanceByStudent/{studentId}")]
        public IActionResult GetAttendanceByStudent(int studentId)
        {
            try
            {
                var attendance = attendances.GetAttendanceByStudent(studentId);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת נוכחות תלמיד: {ex.Message}");
            }
        }

        [HttpGet("student/{studentId}/summary")]
        public IActionResult GetStudentAttendanceSummary(int studentId, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            try
            {
                var summary = attendances.GetStudentAttendanceSummary(studentId, month, year);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת סיכום נוכחות תלמיד: {ex.Message}");
            }
        }

        [HttpGet("student/{studentId}/history")]
        public IActionResult GetStudentAttendanceHistory(int studentId, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            try
            {
                var history = attendances.GetStudentAttendanceHistory(studentId, month, year);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת היסטוריית נוכחות תלמיד: {ex.Message}");
            }
        }

        [HttpGet("reports/monthly")]
        public IActionResult GetMonthlyReport([FromQuery] int month, [FromQuery] int year, [FromQuery] int? groupId = null)
        {
            try
            {
                var report = attendances.GetMonthlyReport(month, year, groupId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת דוח חודשי: {ex.Message}");
            }
        }

        [HttpGet("reports/overall")]
        public IActionResult GetOverallStatistics([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            try
            {
                var statistics = attendances.GetOverallStatistics(month, year);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת סטטיסטיקות כלליות: {ex.Message}");
            }
        }
        [HttpGet("IsAttendanceMarkedForGroup/{groupId}/{date}")]
        public IActionResult IsAttendanceMarkedForGroup(int groupId, string date)
        {
            try
            {
                if (DateOnly.TryParse(date, out DateOnly parsedDate))
                {
                    var attendance = attendances.IsAttendanceMarkedForGroup(groupId, parsedDate);
                    return Ok(attendance);
                }
                return BadRequest("תאריך לא תקין");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת הנתונים האם קיימת נוכחות : {ex.Message}");
            }
        }
        [HttpGet("IsAttendanceMarkedForDay/{date}")]
        public IActionResult IsAttendanceMarkedForDay(string date)
        {
            try
            {
                if (DateOnly.TryParse(date, out DateOnly parsedDate))
                {
                    var attendance = attendances.IsAttendanceMarkedForDay(parsedDate);
                    return Ok(attendance);
                }
                return BadRequest("תאריך לא תקין");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת הנתונים האם קיימת נוכחות : {ex.Message}");
            }
        }
        [HttpPost("AutoMarkDailyAttendance")]
        public IActionResult AutoMarkDailyAttendance()
        {
            try
            {
                attendances.AutoMarkDailyAttendance();
                return Ok("נוכחות יומית סומנה בהצלחה.");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בסימון נוכחות יומית: {ex.Message}");
            }
        }
        [HttpPost("MarkAttendanceForDate")]

        public IActionResult MarkAttendanceForDate(DateOnly date)
        {
            try
            {
                attendances.MarkAttendanceForDate(date);
                return Ok("נוכחות יומית סומנה בהצלחה.");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בסימון נוכחות יומית: {ex.Message}");
            }
        }
        /// <summary>
        /// בדיקת התאריך הראשון שבו נרשמה נוכחות במערכת
        /// </summary>
        [HttpGet]
        [Route("first-attendance-date")]
        public IActionResult GetFirstAttendanceDate()
        {
            try
            {
                var firstDate = attendances.GetFirstAttendanceDate();

                if (!firstDate.HasValue)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "לא נמצאו רשומות נוכחות במערכת",
                        firstDate = (DateOnly?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = $"התאריך הראשון של נוכחות: {firstDate.Value}",
                    firstDate = firstDate.Value,
                    firstDateString = firstDate.Value.ToString("dd/MM/yyyy")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"שגיאה בחיפוש התאריך הראשון: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// סימון נוכחות היסטורי מהתאריך הראשון עד היום
        /// </summary>
        [HttpPost]
        [Route("mark-historical-attendance")]
        public async Task<IActionResult> MarkHistoricalAttendance()
        {
            try
            {
                // מציאת התאריך הראשון
                var firstDate = attendances.GetFirstAttendanceDate();

                if (!firstDate.HasValue)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "לא נמצאו רשומות נוכחות במערכת - אין מה לעדכן"
                    });
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                Console.WriteLine($"🚀 Starting historical attendance marking from: {firstDate.Value} to: {today}");

                // הרצת הסימון ההיסטורי
                bool result = await attendances.MarkHistoricalAttendance(firstDate.Value);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"סימון נוכחות היסטורי הושלם בהצלחה מ-{firstDate.Value} עד {today}",
                        startDate = firstDate.Value.ToString("dd/MM/yyyy"),
                        endDate = today.ToString("dd/MM/yyyy"),
                        totalDays = (today.ToDateTime(TimeOnly.MinValue) - firstDate.Value.ToDateTime(TimeOnly.MinValue)).Days + 1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "סימון נוכחות היסטורי הושלם עם שגיאות - בדוק את הלוגים",
                        startDate = firstDate.Value.ToString("dd/MM/yyyy"),
                        endDate = today.ToString("dd/MM/yyyy")
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"שגיאה בסימון נוכחות היסטורי: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// סימון נוכחות היסטורי עם טווח תאריכים מותאם אישית
        /// </summary>
        [HttpPost]
        [Route("mark-historical-attendance-range")]
        public async Task<IActionResult> MarkHistoricalAttendanceRange([FromBody] HistoricalAttendanceRequest request)
        {
            try
            {
                if (request.StartDate > request.EndDate)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "תאריך התחלה חייב להיות לפני תאריך הסיום"
                    });
                }

                Console.WriteLine($"🚀 Starting custom historical attendance marking from: {request.StartDate} to: {request.EndDate}");

                bool result = await attendances.MarkHistoricalAttendance(request.StartDate, request.EndDate);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"סימון נוכחות היסטורי הושלם בהצלחה מ-{request.StartDate} עד {request.EndDate}",
                        startDate = request.StartDate.ToString("dd/MM/yyyy"),
                        endDate = request.EndDate.ToString("dd/MM/yyyy"),
                        totalDays = (request.EndDate.ToDateTime(TimeOnly.MinValue) - request.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "סימון נוכחות היסטורי הושלם עם שגיאות - בדוק את הלוגים"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"שגיאה בסימון נוכחות היסטורי: {ex.Message}"
                });
            }
        }
    }

    // מודלים לבקשות

    /// <summary>
    /// בקשה לסימון נוכחות היסטורי עם טווח תאריכים
    /// </summary>
    public class HistoricalAttendanceRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
    public class SaveAttendanceRequest
    {
        public int GroupId { get; set; }
        public DateOnly Date { get; set; }
        public List<BLLAttendanceRecord> AttendanceRecords { get; set; } = new();
    }
}
