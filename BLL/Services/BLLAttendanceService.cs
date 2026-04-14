using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLAttendanceService : IBLLAttendance
    {
        private readonly IDAL dal;
        private readonly BLLStudentService studentService;

        public BLLAttendanceService(IDAL dal, BLLStudentService studentService)
        {
            this.dal = dal; 
            this.studentService = studentService;
        }
        #region פונקציות המרה 
        private BLLAttendance ToBLLAttendance(Attendance attendance)
        {
            return new BLLAttendance
            {
                AttendanceId = attendance.AttendanceId,
                LessonId = attendance.LessonId,
                StudentId = attendance.StudentId,
                WasPresent = attendance.WasPresent,
                StatusReport = attendance.StatusReport,
                HealthFundReport = attendance.HealthFundReport,
                DateReport = attendance.DateReport,
                UpdateDate = attendance.UpdateDate,
                UpdateBy = attendance.UpdateBy
            };
        }
        private Attendance ToAttendance(BLLAttendance bllAttendance)
        {
            return new Attendance
            {
                AttendanceId = bllAttendance.AttendanceId,
                LessonId = bllAttendance.LessonId,
                StudentId = bllAttendance.StudentId,
                WasPresent = bllAttendance.WasPresent,
                StatusReport = bllAttendance.StatusReport,
                HealthFundReport = bllAttendance.HealthFundReport,
                DateReport = bllAttendance.DateReport,
                UpdateDate = bllAttendance.UpdateDate,
                UpdateBy = bllAttendance.UpdateBy
            };
        }
        #endregion

        /// <summary>
        ///יצירת נוכחות לתלמיד 
        /// </summary>
        /// <param name="attendance">נוכחות</param>
        public void Create(BLLAttendance attendance)
        {
            dal.Attendances.Create(ToAttendance(attendance));
        }

        /// <summary>
        /// מחיקת נוכחות לתלמיד 
        /// </summary>
        /// <param name="attendance"></param>
        public void Delete(int attendanceId)
        {
            // שליפת רשומת הנוכחות למחיקה
            var attendance = dal.Attendances.GetById(attendanceId);
            if (attendance == null)
                return;

            // מחיקת הנוכחות
            dal.Attendances.Delete(attendanceId);

            //// בדיקה ומחיקה של UnreportedDate עבור אותו תלמיד ואותו תאריך
            //if (attendance.StudentId.HasValue && attendance.Date.HasValue)
            //{
            //    var studentHealthFunds = dal.StudentHealthFunds.GetAll().Result
            //        .Where(shf => shf.StudentId == attendance.StudentId.Value)
            //        .ToList();

            //    foreach (var shf in studentHealthFunds)
            //    {
            //        var unreportedDates = dal.UnreportedDates.GetByStudentHealthFundId(shf.Id);
            //        foreach (var unreported in unreportedDates)
            //        {
            //            if (unreported.DateUnreported != null &&
            //                DateOnly.FromDateTime(unreported.DateUnreported) == attendance.Date.Value)
            //            {
            //                dal.UnreportedDates.Delete(unreported.Id);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Get לכל הנוכחות
        /// </summary>
        /// <returns>List<BLLAttendance></returns>
        public List<BLLAttendance> Get()
        {
            var attendanceRecords = dal.Attendances.Get();
            return attendanceRecords.Select(ToBLLAttendance).ToList();
        }

        /// <summary>
        /// GetById לפי AttendanceId
        /// </summary>
        /// <param name="id">AttendanceId</param>
        /// <returns>BLLAttendancereturns>
        public BLLAttendance GetById(int id)
        {
            var attendance = dal.Attendances.GetById(id);
            return ToBLLAttendance(attendance);
        }

        /// <summary>
        /// עדכון נוכחות תלמיד
        /// </summary>
        /// <param name="attendance"></param>
        public void Update(BLLAttendance attendance)
        {
            dal.Attendances.Update(ToAttendance(attendance));
        }

        /// <summary>
        /// יצירת רשומות בטבלת נוכחות לתלמיד חדש לפי קוד קבוצה
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="groupId"></param>
        public void CreateAttendanceForNewStudentInGroup(int studentId, int groupId, DateOnly enrollmentDate)
        {
            var student = dal.Students.GetById(studentId);

            // שליפת כל השיעורים של הקבוצה החל מתאריך הרישום
            var lessons = dal.Lessons.Get()
                .Where(l => l.GroupId == groupId && l.LessonDate >= enrollmentDate)
                .ToList();

            foreach (var lesson in lessons)
            {
                // בדיקה אם כבר קיימת רשומת נוכחות למניעת כפילויות
                var existing = dal.Attendances.GetAttendanceByGroupAndDate(groupId, lesson.LessonDate)
                    .FirstOrDefault(a => a.LessonId == lesson.LessonId && a.StudentId == studentId);

                if (existing == null)
                {
                    var attendance = new Attendance
                    {
                        LessonId = lesson.LessonId,
                        StudentId = studentId,
                        WasPresent = true, 
                        StatusReport = 3,   // ממתין לדיווח לפי הצורך
                        UpdateDate = DateTime.Now,
                        UpdateBy = null,
                        HealthFundReport = student.HealthFundId, // קופת החולים של התלמיד
                        DateReport = null
                    };
                    dal.Attendances.Create(attendance);
                }
            }
        }

        /// <summary>
        /// עדכון רשומות נוכחות לתלמיד במקרה של שינוי תאריך רישום בקבוצה
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="groupId"></param>
        /// <param name="oldEnrollmentDate"></param>
        /// <param name="newEnrollmentDate"></param>
        public void UpdateAttendancesForEnrollmentDateChange(int studentId, int groupId, DateOnly? oldEnrollmentDate, DateOnly? newEnrollmentDate)
        {
            if (!oldEnrollmentDate.HasValue || !newEnrollmentDate.HasValue || oldEnrollmentDate.Value == newEnrollmentDate.Value)
                return;

            // שליפת כל השיעורים של הקבוצה
            var lessons = dal.Lessons.Get()
                .Where(l => l.GroupId == groupId)
                .ToList();

            // אם התאריך החדש מאוחר יותר - מחיקת נוכחויות עבור שיעורים לפני התאריך החדש
            if (newEnrollmentDate.Value > oldEnrollmentDate.Value)
            {
                var lessonsToRemove = lessons
                    .Where(l => l.LessonDate >= oldEnrollmentDate.Value && l.LessonDate < newEnrollmentDate.Value)
                    .ToList();

                var attendances = dal.Attendances.GetAttendanceByStudentAndDateRange(
                    studentId,
                    oldEnrollmentDate.Value,
                    newEnrollmentDate.Value.AddDays(-1)
                );

                foreach (var lesson in lessonsToRemove)
                {
                    var attendance = attendances.FirstOrDefault(a => a.LessonId == lesson.LessonId && a.StudentId == studentId);
                    if (attendance != null)
                    {
                        dal.Attendances.Delete(attendance.AttendanceId);
                    }
                }
            }
            // אם התאריך החדש מוקדם יותר - הוספת נוכחויות עבור שיעורים בין התאריך החדש לישן
            else if (newEnrollmentDate.Value < oldEnrollmentDate.Value)
            {
                var lessonsToAdd = lessons
                    .Where(l => l.LessonDate >= newEnrollmentDate.Value && l.LessonDate < oldEnrollmentDate.Value)
                    .ToList();

                foreach (var lesson in lessonsToAdd)
                {
                    var existingAttendance = dal.Attendances.GetAttendanceByGroupAndDate(groupId, lesson.LessonDate)
                        .FirstOrDefault(a => a.LessonId == lesson.LessonId && a.StudentId == studentId);

                    if (existingAttendance == null)
                    {
                        var student = dal.Students.GetById(studentId);
                        var attendance = new Attendance
                        {
                            LessonId = lesson.LessonId,
                            StudentId = studentId,
                            WasPresent = false,
                            StatusReport = 3,
                            UpdateDate = DateTime.Now,
                            UpdateBy = null,
                            HealthFundReport = student.HealthFundId,
                            DateReport = lesson.LessonDate
                        };
                        dal.Attendances.Create(attendance);
                    }
                }
            }
        }

        /// <summary>
        /// שליפת היסטוריית נוכחות של תלמיד עם אפשרות לסינון לפי חודש ושנה
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<BLLAttendance> GetStudentAttendanceHistory(int studentId, int? month = null, int? year = null)
        {
            // שליפת כל הנוכחויות של התלמיד
            var attendances = dal.Attendances.GetAttendanceByStudent(studentId).Result;

            // סינון לפי חודש ושנה אם נמסרו
            if (month.HasValue && year.HasValue)
            {
                attendances = attendances
                    .Where(a =>
                        (a.DateReport.HasValue && a.DateReport.Value.Month == month.Value && a.DateReport.Value.Year == year.Value)
                    )
                    .ToList();
            }

            return attendances.Select(ToBLLAttendance).ToList();
        }
        /// <summary>
        /// שליפת נוכחות לתלמיד מסוים
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<List<BLLAttendance>> GetAttendanceByStudent(int studentId)
        {
            try
            {
                var attendanceRecords = await dal.Attendances.GetAttendanceByStudent(studentId);
                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    Console.WriteLine($"No attendance records found for student {studentId}");
                    return new List<BLLAttendance>(); // מחזיר מערך ריק
                }

                return attendanceRecords.Select(a => new BLLAttendance
                {
                    AttendanceId = a.AttendanceId,
                    StudentId = a.StudentId,
                    LessonId = a.LessonId,
                    DateReport = a.DateReport,
                    StatusReport = a.StatusReport,
                    UpdateDate = a.UpdateDate,
                    UpdateBy = a.UpdateBy,
                    HealthFundReport = a.HealthFundReport,
                    WasPresent = a.WasPresent
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance records for student {studentId}: {ex.Message}");
                return new List<BLLAttendance>();
            }
        }

        /// <summary>
        ///  עדכון רשימת נוכחויות של תלמידים ביום מסוים
        /// </summary>
        /// <param name="attendances"></param>
        public void BatchUpdateAttendances(List<BLLAttendance> attendances)
        {
            var dalAttendances = attendances.Select(ToAttendance).ToList();
            dal.Attendances.BatchUpdateAttendances(dalAttendances);
        }



        public BLLStudentAttendanceSummaryDto GetStudentAttendanceSummary(int studentId, int? month = null, int? year = null)
        {
            // שליפת כל הנוכחויות של התלמיד
            var attendances = dal.Attendances.GetAttendanceByStudent(studentId).Result;

            // סינון לפי חודש ושנה אם נמסרו
            if (month.HasValue && year.HasValue)
            {
                attendances = attendances
                    .Where(a =>
                        (a.DateReport.HasValue && a.DateReport.Value.Month == month.Value && a.DateReport.Value.Year == year.Value)
                    )
                    .ToList();
            }

            int total = attendances.Count;
            int present = attendances.Count(a => a.WasPresent);
            int absent = total - present;
            double rate = total > 0 ? (present * 100.0) / total : 0;

            return new BLLStudentAttendanceSummaryDto
            {
                StudentId = studentId,
                TotalLessons = total,
                PresentCount = present,
                AbsentCount = absent,
                AttendanceRate = rate
            };
        }




        public List<BLLAttendanceRecord> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            // שליפת כל רשומות הנוכחות לקבוצה בתאריך המסוים
            var attendanceRecords = dal.Attendances.GetAttendanceByGroupAndDate(groupId, date);

            // המרה ל-BLLAttendanceRecord
            return attendanceRecords.Select(a => new BLLAttendanceRecord
            {
                StudentId = a.StudentId
                // ניתן להוסיף שדות נוספים אם יש צורך
            }).ToList();
        }

        public Dictionary<DateOnly, List<BLLAttendanceRecord>> GetAttendanceByGroupAndDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            // שליפת כל רשומות הנוכחות לקבוצה בטווח תאריכים
            var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(groupId, startDate, endDate);

            // קיבוץ לפי תאריך
            return attendanceRecords
                .GroupBy(a => a.DateReport ?? DateOnly.MinValue)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(a => new BLLAttendanceRecord
                    {
                        StudentId = a.StudentId
                        // ניתן להוסיף שדות נוספים אם יש צורך
                    }).ToList()
                );
        }

        /// <summary>
        /// מחיקת כל רשומות הנוכחות לקבוצה בתאריך מסוים
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date"></param>
        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            dal.Attendances.DeleteByGroupAndDate(groupId, date);
        }

        /// <summary>
        /// יצירת רשומות נוכחות חסרות לכל התלמידים הפעילים בקבוצות שלהם
        /// </summary>
        public void CreateMissingAttendancesForAllActiveStudents()
        {
            var startedAt = DateTime.Now;
            Console.WriteLine("---- BLL START CreateMissingAttendancesForAllActiveStudents ----");
            Console.WriteLine($"StartedAt: {startedAt:yyyy-MM-dd HH:mm:ss.fff}");

            try
            {
                Console.WriteLine("Step 1: loading students...");
                var allStudents = dal.Students.Get().ToList();
                Console.WriteLine($"Step 1 OK: students count = {allStudents.Count}");

                Console.WriteLine("Step 2: loading active group-students...");
                // Fix for CS0029 and CS1662 errors
                // The issue is that `IsActive` is of type `byte?` and cannot be directly used as a `bool`.
                // We need to explicitly check if `IsActive` has a value and if that value is non-zero.

                var allActiveGroupStudents = dal.GroupStudents.Get()
                    .Where(gs => gs.IsActive.HasValue && gs.IsActive.Value != 0)
                    .ToList();
                Console.WriteLine($"Step 2 OK: active group-students count = {allActiveGroupStudents.Count}");

                Console.WriteLine("Step 3: building studentsWithActiveEnrollment set...");
                var studentsWithActiveEnrollment = allActiveGroupStudents
                    .Select(gs => gs.StudentId)
                    .ToHashSet();
                Console.WriteLine($"Step 3 OK: unique students with active enrollment = {studentsWithActiveEnrollment.Count}");

                Console.WriteLine("Step 4: filtering relevant students...");
                var relevantStudents = allStudents
                    .Where(s =>
                     studentService.GetStudentStatus(s.Id) =="פעיל" ||
                        studentsWithActiveEnrollment.Contains(s.Id))
                    .ToList();
                Console.WriteLine($"Step 4 OK: ");

                if (!relevantStudents.Any())
                {
                    Console.WriteLine("No relevant students found. Exiting.");
                    return;
                }

                var relevantStudentIds = relevantStudents.Select(s => s.Id).ToHashSet();

                Console.WriteLine("Step 5: loading lessons...");
                var allLessons = dal.Lessons.Get().ToList();
                Console.WriteLine($"Step 5 OK: lessons count = {allLessons.Count}");

                var lessonsByGroup = allLessons
                    .GroupBy(l => l.GroupId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(l => l.LessonId).ToHashSet()
                    );
                Console.WriteLine($"Step 5 OK: groups with lessons = {lessonsByGroup.Count}");

                Console.WriteLine("Step 6: loading attendance for relevant students...");
                List<dynamic> allRelevantAttendances = null;

                try
                {
                    allRelevantAttendances = dal.Attendances.Get()
                        .Where(a => relevantStudentIds.Contains(a.StudentId))
                        .Select(a => new { a.StudentId, a.LessonId })
                        .ToList<dynamic>();

                    Console.WriteLine($"Step 6 OK: attendance rows loaded = {allRelevantAttendances.Count}");
                }
                catch (Exception exAttendanceLoad)
                {
                    Console.WriteLine("Step 6 FAILED while retrieving attendance records");
                    Console.WriteLine($"Step 6 Error Message: {exAttendanceLoad.Message}");
                    Console.WriteLine($"Step 6 Error Type: {exAttendanceLoad.GetType().FullName}");
                    Console.WriteLine($"Step 6 StackTrace: {exAttendanceLoad.StackTrace}");
                    throw;
                }

                Console.WriteLine("Step 7: building attendedLessonsByStudent map...");
                var attendedLessonsByStudent = allRelevantAttendances
                    .GroupBy(a => (int)a.StudentId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => (int)x.LessonId).ToHashSet()
                    );
                Console.WriteLine($"Step 7 OK: students with attendance map entries = {attendedLessonsByStudent.Count}");

                var enrollmentsToProcess = allActiveGroupStudents
                    .Where(gs => relevantStudentIds.Contains(gs.StudentId))
                    .ToList();
                Console.WriteLine($"Step 8: enrollments to process = {enrollmentsToProcess.Count}");

                int createdCalls = 0;
                int skippedHasAttendance = 0;
                int skippedNoLessons = 0;
                int failedCreateCalls = 0;

                Console.WriteLine("Step 9: processing enrollments...");
                foreach (var enrollment in enrollmentsToProcess)
                {
                    int studentId = enrollment.StudentId;
                    int groupId = enrollment.GroupId;

                    if (!lessonsByGroup.TryGetValue(groupId, out var groupLessonIds) || !groupLessonIds.Any())
                    {
                        skippedNoLessons++;
                        Console.WriteLine($"Skip: studentId={studentId}, groupId={groupId}, reason=No lessons in group");
                        continue;
                    }

                    bool hasAttendanceInGroup =
                        attendedLessonsByStudent.TryGetValue(studentId, out var studentLessons)
                        && studentLessons.Any(lid => groupLessonIds.Contains(lid));

                    if (hasAttendanceInGroup)
                    {
                        skippedHasAttendance++;
                        Console.WriteLine($"Skip: studentId={studentId}, groupId={groupId}, reason=Already has attendance in group");
                        continue;
                    }

                    try
                    {
                        DateOnly? fromDate = enrollment.EnrollmentDate;


                        Console.WriteLine($"Create: studentId={studentId}, groupId={groupId}, fromDate={(fromDate.HasValue ? fromDate.Value.ToString() : "null")}");
                        CreateAttendanceForNewStudentInGroup(studentId, groupId, (DateOnly)fromDate);
                        createdCalls++;
                    }
                    catch (Exception exCreate)
                    {
                        failedCreateCalls++;
                        Console.WriteLine($"Create FAILED: studentId={studentId}, groupId={groupId}");
                        Console.WriteLine($"Create Error Message: {exCreate.Message}");
                        Console.WriteLine($"Create Error Type: {exCreate.GetType().FullName}");
                        Console.WriteLine($"Create StackTrace: {exCreate.StackTrace}");
                    }
                }

                var finishedAt = DateTime.Now;
                Console.WriteLine("Step 10: summary");
                Console.WriteLine($"Created calls: {createdCalls}");
                Console.WriteLine($"Skipped (already has attendance): {skippedHasAttendance}");
                Console.WriteLine($"Skipped (no lessons in group): {skippedNoLessons}");
                Console.WriteLine($"Failed create calls: {failedCreateCalls}");
                Console.WriteLine($"Duration seconds: {(finishedAt - startedAt).TotalSeconds:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL FATAL ERROR");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().FullName}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                Console.WriteLine($"EndedAt: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                Console.WriteLine("---- BLL END CreateMissingAttendancesForAllActiveStudents ----");
            }

        }

        /// <summary>
        /// שליפת היסטוריית נוכחות של תלמיד עם אפשרות לסינון לפי חודש ושנה - גרסה אסינכרונית
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<BLLAttendance>> GetStudentAttendanceHistoryAsync(int studentId, int? month = null, int? year = null)
        {
            var attendances = await dal.Attendances.GetStudentAttendanceHistoryAsync(studentId, month, year);
            return attendances.Select(ToBLLAttendance).ToList();
        }

        /// <summary>
        /// שליפת סיכום נוכחות של תלמיד עם אפשרות לסינון לפי חודש ושנה - גרסה אסינכרונית
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<BLLStudentAttendanceSummaryDto> GetStudentAttendanceSummaryAsync(int studentId, int? month = null, int? year = null)
        {
            Console.WriteLine($"[BLL] GetStudentAttendanceSummaryAsync called. studentId={studentId}, month={month}, year={year}");
            var (totalLessons, presentCount) = await dal.Attendances.GetStudentAttendanceSummaryDataAsync(studentId, month, year);

            var absentCount = totalLessons - presentCount;
            var attendanceRate = totalLessons == 0 ? 0 : (presentCount * 100.0) / totalLessons;

            Console.WriteLine($"[BLL] Summary: TotalLessons={totalLessons}, PresentCount={presentCount}, AbsentCount={absentCount}, AttendanceRate={attendanceRate}");

            return new BLLStudentAttendanceSummaryDto
            {
                StudentId = studentId,
                TotalLessons = totalLessons,
                PresentCount = presentCount,
                AbsentCount = absentCount,
                AttendanceRate = attendanceRate
            };
        }
    }
}
