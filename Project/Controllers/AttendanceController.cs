using BLL.Api;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Delete(BLLAttendance attendance)
        {
            try
            {
                attendances.Delete(attendance);
                return Ok("נוכחות נמחקה בהצלחה");
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

        //[HttpGet("GetAttendanceByStudent/{studentId}")]
        //public IActionResult GetAttendanceByStudent(int studentId)
        //{
        //    try
        //    {
        //        var attendance = attendances.GetAttendanceByStudent(studentId);
        //        return Ok(attendance);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"שגיאה בקבלת נוכחות תלמיד: {ex.Message}");
        //    }
        //}

        [HttpGet("GetAttendanceByStudentAndDateRange/{studentId}/{startDate}/{endDate}")]
        public IActionResult GetAttendanceByStudentAndDateRange(int studentId, string startDate, string endDate)
        {
            try
            {
                if (DateOnly.TryParse(startDate, out DateOnly start) &&
                    DateOnly.TryParse(endDate, out DateOnly end))
                {
                    var attendance = attendances.GetAttendanceByGroupAndDateRange(studentId, start, end);
                    return Ok(attendance);
                }
                return BadRequest("תאריכים לא תקינים");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בקבלת נוכחות תלמיד לטווח תאריכים: {ex.Message}");
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
    }

    // מודלים לבקשות
    public class SaveAttendanceRequest
    {
        public int GroupId { get; set; }
        public DateOnly Date { get; set; }
        public List<BLLAttendanceRecord> AttendanceRecords { get; set; } = new();
    }
}
