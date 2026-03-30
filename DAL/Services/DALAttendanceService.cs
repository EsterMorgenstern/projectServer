using Microsoft.EntityFrameworkCore;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALAttendanceService : IDALAttendance
    {
        dbcontext dbcontext;

        public DALAttendanceService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Attendance attendance)
        {
            try
            {
                attendance.AttendanceId = 0; // חובה לרשומה חדשה עם Identity
                dbcontext.Attendances.Add(attendance);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:O}] ERROR DAL.Attendances.Create - entity: {ex}");
                throw;
            }
        }

        public void Delete(int attendanceId)
        {
            var trackedAttendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId == attendanceId);
            if (trackedAttendance != null)
            {
                dbcontext.Attendances.Remove(trackedAttendance);
                dbcontext.SaveChanges();
            }
        }

        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
        }

        //public void DeleteByGroupAndDate(int groupId, DateOnly date)
        //{
        //    var attendanceToDelete = dbcontext.Attendances.Where(x => x.GroupId == groupId && x.Date == date).ToList();
        //    if (attendanceToDelete.Any())
        //    {
        //        dbcontext.Attendances.RemoveRange(attendanceToDelete);
        //        dbcontext.SaveChanges();
        //    }
        //}

        public List<Attendance> Get()
        {
            try
            {
                if (dbcontext.Attendances == null)
                    return new List<Attendance>();

                var attendances = dbcontext.Attendances.ToList();
                return attendances;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving attendance records.", ex);
            }
        }

        public List<Attendance> GetAttendanceByGroup(int groupId)
        {
            if (groupId <= 0)
                return new List<Attendance>();

            // שלוף את כל מזהי השיעורים של הקבוצה
            var lessonIds = dbcontext.Lessons
                .Where(l => l.GroupId == groupId)
                .Select(l => l.LessonId)
                .ToList();

            if (!lessonIds.Any())
                return new List<Attendance>();

            // שלוף את כל הנוכחויות עבור אותם שיעורים
            return dbcontext.Attendances
                .Where(a => lessonIds.Contains(a.LessonId))
                .ToList();
        }

       public List<Attendance> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            try
            {
                if (groupId <= 0)
                    return new List<Attendance>();

                // אם LessonDate הוא DateTime ב-DB:
                var lessonIds = dbcontext.Lessons
                    .Where(l => l.GroupId == groupId && l.LessonDate == date)
                    .Select(l => l.LessonId)
                    .ToList();

           
                if (!lessonIds.Any())
                    return new List<Attendance>();

                return dbcontext.Attendances
                    .Where(a => lessonIds.Contains(a.LessonId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"An error occurred while retrieving attendance records for groupId={groupId}, date={date}.",
                    ex
                );
            }
        }

        public async Task<List<Attendance>> GetAttendanceByStudent(int studentId)
        {
            return await Task.Run(() => dbcontext.Attendances.Where(x => x.StudentId == studentId).ToList());
        }

        public List<Attendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        public List<Attendance> GetByGroupAndDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetById לפי AttendanceId
        /// </summary>
        /// <param name="id">AttendanceId</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Attendance GetById(int id)
        {
            var attendance = dbcontext.Attendances.SingleOrDefault(x => x.AttendanceId == id);
            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }
            return attendance;
        }

        /// <summary>
        /// עדכון נוכחות 
        /// </summary>
        /// <param name="attendance"></param>
        public void Update(Attendance attendance)
        {
            var tracked = dbcontext.Attendances.SingleOrDefault(a => a.AttendanceId == attendance.AttendanceId);
            if (tracked != null)
            {
                tracked.LessonId = attendance.LessonId;
                tracked.StudentId = attendance.StudentId;
                tracked.WasPresent = attendance.WasPresent;
                tracked.StatusReport = attendance.StatusReport;
                tracked.HealthFundReport = attendance.HealthFundReport;
                tracked.DateReport = attendance.DateReport;
                tracked.UpdateDate = attendance.UpdateDate;
                tracked.UpdateBy = attendance.UpdateBy;
                dbcontext.SaveChanges();
            }
        }

        /// <summary>
        /// עדכון רשימת נוכחויות 
        /// </summary>
        /// <param name="attendances"></param>
        public void BatchUpdateAttendances(List<Attendance> attendances)
        {
            foreach (var att in attendances)
            {
                var existing = GetByLessonAndStudent(att.LessonId, att.StudentId);
                if (existing != null)
                {
                    att.AttendanceId = existing.AttendanceId;
                    Update(att);
                }
                else
                {
                    Create(att);
                }
            }
        }
        /// <summary>
        /// שליפת נתוני נוכחות של תלמיד מסוים
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Attendance>> GetStudentAttendanceHistoryAsync(int studentId, int? month = null, int? year = null)
        {
            try
            {
                if (studentId <= 0)
                    return new List<Attendance>();

                IQueryable<Attendance> query = dbcontext.Attendances
                    .AsNoTracking()
                    .Where(a => a.StudentId == studentId);

                if (year.HasValue)
                {
                    query = query.Where(a => a.DateReport.HasValue && a.DateReport.Value.Year == year.Value);
                }

                if (month.HasValue)
                {
                    query = query.Where(a => a.DateReport.HasValue && a.DateReport.Value.Month == month.Value);
                }

                return await query
                    .OrderByDescending(a => a.DateReport)
                    .ThenByDescending(a => a.AttendanceId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"An error occurred while retrieving attendance history for studentId={studentId}, month={month}, year={year}.",
                    ex
                );
            }
        }

        /// <summary>
        /// החזרת סיכום על נוכחות של תלמיד מסוים עם או בלי שנה וחודש
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<(int TotalLessons, int PresentCount)> GetStudentAttendanceSummaryDataAsync(int studentId, int? month = null, int? year = null)
        {
            Console.WriteLine($"[DAL] GetStudentAttendanceSummaryDataAsync called. studentId={studentId}, month={month}, year={year}");
            try
            {
                if (studentId <= 0)
                {
                    Console.WriteLine("[DAL] studentId <= 0, returning (0,0)");
                    return (0, 0);
                }

                IQueryable<Attendance> query = dbcontext.Attendances
                    .AsNoTracking()
                    .Where(a => a.StudentId == studentId);

                if (year.HasValue)
                {
                    query = query.Where(a => a.DateReport.HasValue && a.DateReport.Value.Year == year.Value);
                    Console.WriteLine($"[DAL] Filtered by year: {year.Value}");
                }

                if (month.HasValue)
                {
                    query = query.Where(a => a.DateReport.HasValue && a.DateReport.Value.Month == month.Value);
                    Console.WriteLine($"[DAL] Filtered by month: {month.Value}");
                }

                var summary = await query
                    .GroupBy(_ => 1)
                    .Select(g => new
                    {
                        TotalLessons = g.Count(),
                        PresentCount = g.Count(x => x.WasPresent)
                    })
                    .FirstOrDefaultAsync();

                if (summary == null)
                {
                    Console.WriteLine("[DAL] No attendance records found, returning (0,0)");
                    return (0, 0);
                }

                Console.WriteLine($"[DAL] Summary: TotalLessons={summary.TotalLessons}, PresentCount={summary.PresentCount}");
                return (summary.TotalLessons, summary.PresentCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DAL] ERROR in GetStudentAttendanceSummaryDataAsync: {ex.Message}");
                throw new InvalidOperationException(
                    $"An error occurred while retrieving attendance summary for studentId={studentId}, month={month}, year={year}.",
                    ex
                );
            }
        }

        /// <summary>
        /// פונקציה עזר לשליפת נוכחות לפי שיעור ותלמיד
        /// </summary>
        /// <param name="lessonId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Attendance? GetByLessonAndStudent(int lessonId, int studentId)
        {
            return dbcontext.Attendances.FirstOrDefault(a => a.LessonId == lessonId && a.StudentId == studentId);
        }


    }
}