using System;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLAttendanceService : IBLLAttendance
    {
        private readonly IDAL dal;

        public BLLAttendanceService(IDAL dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// נוכחות לתלמיד
        /// </summary>
        /// <param name="attendance">נוכחות</param>

        public void Create(BLLAttendance attendance)
        {
            Attendance a = new Attendance()
            {
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent,
                Date = attendance.Date
            };
            dal.Attendances.Create(a);
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

            // בדיקה ומחיקה של UnreportedDate עבור אותו תלמיד ואותו תאריך
            if (attendance.StudentId.HasValue && attendance.Date.HasValue)
            {
                var studentHealthFunds = dal.StudentHealthFunds.GetAll().Result
                    .Where(shf => shf.StudentId == attendance.StudentId.Value)
                    .ToList();

                foreach (var shf in studentHealthFunds)
                {
                    var unreportedDates = dal.UnreportedDates.GetByStudentHealthFundId(shf.Id);
                    foreach (var unreported in unreportedDates)
                    {
                        if (unreported.DateUnreported != null &&
                            DateOnly.FromDateTime(unreported.DateUnreported) == attendance.Date.Value)
                        {
                            dal.UnreportedDates.Delete(unreported.Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get לכל הנוכחות
        /// </summary>
        /// <returns>List<BLLAttendance></returns>
        public List<BLLAttendance> Get()
        {
            try
            {
                var attendanceRecords = dal.Attendances.Get();
                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    Console.WriteLine("No attendance records found.");
                    return new List<BLLAttendance>(); // מחזיר מערך ריק
                }

                return attendanceRecords.Select(b => new BLLAttendance()
                {
                    Date = b.Date,
                    StudentId = b.StudentId,
                    AttendanceId = b.AttendanceId,
                    GroupId = b.GroupId,
                    WasPresent = b.WasPresent
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance records: {ex.Message}");
                return new List<BLLAttendance>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        /// <summary>
        /// GetById לפי AttendanceId
        /// </summary>
        /// <param name="id">AttendanceId</param>
        /// <returns>BLLAttendancereturns>
        public BLLAttendance GetById(int id)
        {
            var attendance = dal.Attendances.GetById(id);
            return new BLLAttendance()
            {
                AttendanceId = attendance.AttendanceId,
                Date = attendance.Date,
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent
            };
        }

        public void Update(BLLAttendance attendance)
        {
            Attendance a = new Attendance()
            {
                AttendanceId = attendance.AttendanceId,
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent,
                Date = attendance.Date
            };
            dal.Attendances.Update(a);
        }
        /// <summary>
        /// עבור תאריך מסוים מחיקת הנוכחות הקיימת ויצירת נוכחות חדשה לכל התלמידים
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date"></param>
        /// <param name="attendanceRecords"></param>
        /// <returns>האם הפעולה הצליחה</returns>

        public bool SaveAttendanceForDate(int groupId, DateOnly date, List<BLLAttendanceRecord> attendanceRecords)
        {
            try
            {
                dal.Attendances.DeleteByGroupAndDate(groupId, date);

                foreach (var record in attendanceRecords)
                {
                    var student = dal.Students.GetById(record.StudentId);
                    if (student.Status == "פעיל") // בדיקה אם התלמיד פעיל
                    {
                        dal.Attendances.Create(new Attendance
                        {
                            GroupId = groupId,
                            StudentId = record.StudentId,
                            Date = date,
                            WasPresent = record.WasPresent
                        });
                    }
                }

                var group = dal.Groups.GetById(groupId);
                if (group != null)
                {
                    group.LessonsCompleted = (group.LessonsCompleted ?? 0) + 1;
                    dal.Groups.Update(group);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogExceptionWithContext(ex, $"SaveAttendanceForDate(groupId={groupId}, date={date})",
                    new { groupId, date, attendanceCount = attendanceRecords?.Count ?? 0 });
                return false;
            }
        }
        /// <summary>
        /// בדיקה אם קיימת נוכחות עבור הקבוצה בתאריך המסוים
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsAttendanceMarkedForGroup(int groupId, DateOnly date)
        {
            try
            {
                return dal.Attendances.GetAttendanceByGroupAndDate(groupId, date).Any();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking attendance for group {groupId} on date {date}: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// המרת תאריך ליום עברי
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetHebrewDayName(DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => "ראשון",
                DayOfWeek.Monday => "שני",
                DayOfWeek.Tuesday => "שלישי",
                DayOfWeek.Wednesday => "רביעי",
                DayOfWeek.Thursday => "חמישי",
                DayOfWeek.Friday => "שישי",
                DayOfWeek.Saturday => "שבת",
                _ => throw new ArgumentException("Invalid day of week")
            };
        }
        /// <summary>
        /// בדיקה ליום מסוים אם סומן בו נוכחות לכל הקבוצות
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsAttendanceMarkedForDay(DateOnly date)
        {
            try
            {
                string dayName = GetHebrewDayName(date);
                Console.WriteLine($"🔍 Checking attendance for day: {date} ({dayName})");

                var groups = dal.Groups.GetGroupsByDayOfWeek(dayName);

                // בדיקה אם יש קבוצות בכלל
                if (groups == null || !groups.Any())
                {
                    Console.WriteLine($"No groups found for date {date}");
                    return true;
                }

                Console.WriteLine($"Found {groups.Count()} groups for {dayName}");

                // בדיקה עבור כל קבוצה - עם פירוט
                var groupsWithoutAttendance = new List<int>();

                foreach (var item in groups)
                {
                    var hasAttendance = IsAttendanceMarkedForGroup(item.GroupId, date);
                    Console.WriteLine($"   Group {item.GroupId}: HasAttendance = {hasAttendance}");

                    if (!hasAttendance)
                    {
                        groupsWithoutAttendance.Add(item.GroupId);
                        Console.WriteLine($"❌ Attendance not marked for group {item.GroupId} on {date}");
                    }
                }

                if (groupsWithoutAttendance.Any())
                {
                    Console.WriteLine($"❌ Missing attendance for {groupsWithoutAttendance.Count} groups: [{string.Join(", ", groupsWithoutAttendance)}]");
                    return false;
                }

                Console.WriteLine($"✅ Attendance marked for all {groups.Count()} groups on {date}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking attendance on date {date}: {ex.Message}");
                return false;
            }
        }

        public List<BLLAttendanceRecord> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            try
            {
                var attendanceRecords = dal.Attendances.GetAttendanceByGroupAndDate(groupId, date);
                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    Console.WriteLine($"No attendance records found for group {groupId} on {date}");
                    return new List<BLLAttendanceRecord>(); // מחזיר מערך ריק
                }

                return attendanceRecords.Select(a => new BLLAttendanceRecord
                {
                    StudentId = a.StudentId ?? 0,
                    WasPresent = a.WasPresent ?? false,
                    StudentName = GetStudentName(a.StudentId ?? 0)
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance records for group {groupId} on {date}: {ex.Message}");
                return new List<BLLAttendanceRecord>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        public Dictionary<DateOnly, List<BLLAttendanceRecord>> GetAttendanceByGroupAndDateRange(
            int groupId, DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(groupId, startDate, endDate);
                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    Console.WriteLine($"No attendance records found for group {groupId} between {startDate} and {endDate}");
                    return new Dictionary<DateOnly, List<BLLAttendanceRecord>>(); // מחזיר מילון ריק
                }

                return attendanceRecords.GroupBy(a => a.Date)
                    .ToDictionary(
                        group => group.Key ?? DateOnly.MinValue,
                        group => group.Select(a => new BLLAttendanceRecord
                        {
                            StudentId = a.StudentId ?? 0,
                            WasPresent = a.WasPresent ?? false,
                            StudentName = GetStudentName(a.StudentId ?? 0)
                        }).ToList()
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance records for group {groupId} between {startDate} and {endDate}: {ex.Message}");
                return new Dictionary<DateOnly, List<BLLAttendanceRecord>>(); // מחזיר מילון ריק במקרה של שגיאה
            }
        }

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
                    GroupId = a.GroupId,
                    Date = a.Date,
                    WasPresent = a.WasPresent
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attendance records for student {studentId}: {ex.Message}");
                return new List<BLLAttendance>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }
        /// <summary>
        /// קבלת נוכחות עבור תלמיד בטווח תאריכים
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>

        public List<BLLAttendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByStudentAndDateRange(studentId, startDate, endDate);
            return attendanceRecords.Select(a => new BLLAttendance
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                GroupId = a.GroupId,
                Date = a.Date,
                WasPresent = a.WasPresent
            }).ToList();
        }

        public BLLAttendanceStatistics GetAttendanceStatistics(int groupId)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByGroup(groupId);
            var groupInfo = dal.Groups.GetById(groupId);
            var groupStudents = dal.Groups.GetStudentsByGroupId(groupId);

            var studentStats = new List<BLLStudentAttendanceStats>();

            foreach (var student in groupStudents)
            {
                var studentAttendance = attendanceRecords.Where(a => a.StudentId == student.StudentId).ToList();
                var attendedLessons = studentAttendance.Count(a => a.WasPresent == true);
                var totalLessons = studentAttendance.Count;

                studentStats.Add(new BLLStudentAttendanceStats
                {
                    StudentId = student.StudentId,
                    StudentName = GetStudentName(student.StudentId),
                    TotalLessons = totalLessons,
                    AttendedLessons = attendedLessons,
                    AttendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0
                });
            }

            return new BLLAttendanceStatistics
            {
                GroupId = groupId,
                GroupName = groupInfo.GroupName,
                TotalStudents = groupStudents.Count,
                TotalLessons = attendanceRecords.GroupBy(a => a.Date).Count(),
                AverageAttendanceRate = studentStats.Any() ? studentStats.Average(s => s.AttendanceRate) : 0,
                StudentStats = studentStats
            };
        }

        public BLLAttendanceStatistics GetAttendanceStatisticsByDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(groupId, startDate, endDate);
            var groupInfo = dal.Groups.GetById(groupId);
            var groupStudents = dal.Groups.GetStudentsByGroupId(groupId);

            var studentStats = new List<BLLStudentAttendanceStats>();

            foreach (var student in groupStudents)
            {
                var studentAttendance = attendanceRecords.Where(a => a.StudentId == student.StudentId).ToList();
                var attendedLessons = studentAttendance.Count(a => a.WasPresent == true);
                var totalLessons = studentAttendance.Count;

                studentStats.Add(new BLLStudentAttendanceStats
                {
                    StudentId = student.StudentId,
                    StudentName = GetStudentName(student.StudentId),
                    TotalLessons = totalLessons,
                    AttendedLessons = attendedLessons,
                    AttendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0
                });
            }

            return new BLLAttendanceStatistics
            {
                GroupId = groupId,
                GroupName = groupInfo.GroupName,
                TotalStudents = groupStudents.Count,
                TotalLessons = attendanceRecords.GroupBy(a => a.Date).Count(),
                AverageAttendanceRate = studentStats.Any() ? studentStats.Average(s => s.AttendanceRate) : 0,
                StudentStats = studentStats
            };
        }
        /// <summary>
        /// מחיקת נוכחויות לפי קבוצה ותאריך
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool DeleteAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            try
            {
                dal.Attendances.DeleteByGroupAndDate(groupId, date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // פונקציה עזר לקבלת שם תלמיד
        private string GetStudentName(int studentId)
        {
            try
            {
                var student = dal.Students.GetById(studentId);
                return $"{student.FirstName} {student.LastName}";
            }
            catch
            {
                return "תלמיד לא נמצא";
            }
        }

        public async Task<BLLStudentAttendanceSummary> GetStudentAttendanceSummary(int studentId, int? month = null, int? year = null)
        {
            var student = dal.Students.GetById(studentId);
            var attendanceRecords = await dal.Attendances.GetAttendanceByStudent(studentId);

            if (month.HasValue && year.HasValue)
            {
                attendanceRecords = attendanceRecords.Where(a =>
                    a.Date.HasValue &&
                    a.Date.Value.Month == month.Value &&
                    a.Date.Value.Year == year.Value).ToList();
            }

            var totalLessons = attendanceRecords.Count;
            var attendedLessons = attendanceRecords.Count(a => a.WasPresent == true);
            var absentLessons = totalLessons - attendedLessons;
            var attendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0;

            return new BLLStudentAttendanceSummary
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                TotalLessons = totalLessons,
                AttendedLessons = attendedLessons,
                AbsentLessons = absentLessons,
                AttendanceRate = Math.Round(attendanceRate, 1),
                Month = month ?? DateTime.Now.Month,
                Year = year ?? DateTime.Now.Year
            };
        }

        public async Task<List<BLLStudentAttendanceHistory>> GetStudentAttendanceHistory(int studentId, int? month = null, int? year = null)
        {
            var attendanceRecords = await dal.Attendances.GetAttendanceByStudent(studentId);

            if (month.HasValue && year.HasValue)
            {
                attendanceRecords = attendanceRecords.Where(a =>
                    a.Date.HasValue &&
                    a.Date.Value.Month == month.Value &&
                    a.Date.Value.Year == year.Value).ToList();
            }

            var result = new List<BLLStudentAttendanceHistory>();

            foreach (var record in attendanceRecords)
            {
                try
                {
                    var group = dal.Groups.GetById(record.GroupId ?? 0);
                    var course = dal.Courses.GetById(group.CourseId);
                    var branch = dal.Branches.GetById(group.BranchId);
                    var instructor = dal.Instructors.GetById(group.InstructorId);

                    result.Add(new BLLStudentAttendanceHistory
                    {
                        AttendanceId = record.AttendanceId,
                        StudentId = studentId,
                        StudentName = GetStudentName(studentId),
                        GroupId = record.GroupId ?? 0,
                        GroupName = group.GroupName ?? "",
                        CourseName = course.CouresName ?? "",
                        InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                        BranchName = branch.Name ?? "",
                        Date = record.Date ?? DateOnly.MinValue,
                        LessonTime = group.Hour,
                        IsPresent = record.WasPresent ?? false,

                    });
                }
                catch (Exception ex)
                {
                    // לוג השגיאה אבל המשך עם הרשומות האחרות
                    Console.WriteLine($"Error processing attendance record {record.AttendanceId}: {ex.Message}");
                }
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        public BLLMonthlyReport GetMonthlyReport(int month, int year, int? groupId = null)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var groups = groupId.HasValue ?
                new List<DAL.Models.Group> { dal.Groups.GetById(groupId.Value) } :
                dal.Groups.Get();

            var groupReports = new List<BLLGroupReport>();
            var totalStudents = 0;
            var totalLessons = 0;
            var totalAttendanceRecords = 0;
            var totalPresentRecords = 0;

            foreach (var group in groups)
            {
                try
                {
                    var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(group.GroupId, startDate, endDate);
                    var groupStudents = dal.Groups.GetStudentsByGroupId(group.GroupId);
                    var course = dal.Courses.GetById(group.CourseId);
                    var branch = dal.Branches.GetById(group.BranchId);

                    var groupTotalLessons = attendanceRecords.GroupBy(a => a.Date).Count();
                    var groupTotalAttendance = attendanceRecords.Count;
                    var groupPresentCount = attendanceRecords.Count(a => a.WasPresent == true);
                    var groupAttendanceRate = groupTotalAttendance > 0 ?
                        (double)groupPresentCount / groupTotalAttendance * 100 : 0;

                    groupReports.Add(new BLLGroupReport
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName ?? "",
                        CourseName = course.CouresName ?? "",
                        BranchName = branch.Name ?? "",
                        TotalStudents = groupStudents.Count,
                        TotalLessons = groupTotalLessons,
                        AverageAttendanceRate = Math.Round(groupAttendanceRate, 1)
                    });

                    totalStudents += groupStudents.Count;
                    totalLessons += groupTotalLessons;
                    totalAttendanceRecords += groupTotalAttendance;
                    totalPresentRecords += groupPresentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing group {group.GroupId}: {ex.Message}");
                }
            }

            var overallAttendanceRate = totalAttendanceRecords > 0 ?
                (double)totalPresentRecords / totalAttendanceRecords * 100 : 0;

            return new BLLMonthlyReport
            {
                Month = month,
                Year = year,
                OverallStatistics = new BLLOverallStatistics
                {
                    TotalStudents = totalStudents,
                    TotalLessons = totalLessons,
                    TotalGroups = groups.Count,
                    OverallAttendanceRate = Math.Round(overallAttendanceRate, 1)
                },
                Groups = groupReports
            };
        }

        public BLLOverallStatistics GetOverallStatistics(int? month = null, int? year = null)
        {
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            var report = GetMonthlyReport(currentMonth, currentYear);
            return report.OverallStatistics;
        }

        /// <summary>
        /// מחיקת נוכחות לפי קבוצה ותאריך
        /// </summary>
        /// <param name="groupId">מזהה הקבוצה</param>
        /// <param name="date">התאריך למחיקה</param>
        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            try
            {
                // מציאת כל רשומות הנוכחות לקבוצה בתאריך המסוים
                var attendancesToDelete = dal.Attendances.Get()
                    .Where(a => a.GroupId == groupId &&
                               a.Date.HasValue &&
                               a.Date.Value == date)
                    .ToList();

                if (attendancesToDelete.Any())
                {
                    Console.WriteLine($"Deleting {attendancesToDelete.Count} attendance records for group {groupId} on {date}");

                    // מחיקת כל הרשומות
                    foreach (var attendance in attendancesToDelete)
                    {
                        dal.Attendances.Delete(attendance.AttendanceId);
                    }

                    Console.WriteLine($"✅ Successfully deleted attendance records for group {groupId} on {date}");
                }
                else
                {
                    Console.WriteLine($"No attendance records found for group {groupId} on {date} - nothing to delete");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting attendance for group {groupId} on {date}: {ex.Message}");
                throw; // זורק את השגיאה הלאה כדי שהפונקציה הקוראת תדע שהיתה בעיה
            }
        }



        /// <summary>
        /// בדיקה אם יש ביטול לקבוצה ביום מסוים
        /// </summary>
        /// <param name="groupId">מזהה הקבוצה</param>
        /// <param name="date">תאריך</param>
        /// <returns>האם הקבוצה בוטלה</returns>
        private bool IsGroupCanceledForDay(int groupId, DateOnly date)
        {
            try
            {
                return dal.LessonCancellations.GetCancellationsByDate(date.ToDateTime(TimeOnly.MinValue))
                    .Any(cancellation => cancellation.GroupId == groupId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking cancellation for group {groupId} on {date}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// בדיקה אם יש נוכחות מסומנת לתלמיד בקבוצה ביום מסוים
        /// </summary>
        /// <param name="studentId">מזהה התלמיד</param>
        /// <param name="groupId">מזהה הקבוצה</param>
        /// <param name="date">תאריך</param>
        /// <returns>האם יש נוכחות מסומנת</returns>
        private bool IsAttendanceMarkedForStudent(int studentId, int groupId, DateOnly date)
        {
            try
            {
                return dal.Attendances.GetAttendanceByGroupAndDate(groupId, date)
                    .Any(attendance => attendance.StudentId == studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking attendance for student {studentId} in group {groupId} on {date}: {ex.Message}");
                return false;
            }
        }

        public async Task AddUnreportedDateByAttendance(DateOnly attendanceDate, int studentId)
        {
            await AddUnreportedDateByAttendanceWithDuplicateCheck(attendanceDate, studentId);
        }

        /// <summary>
        /// פונקציה מרכזית לסימון נוכחות עבור תאריך מסוים
        /// </summary>
        /// <param name="date">התאריך לסימון</param>
        /// <param name="operationName">שם הפעולה ללוג</param>
        /// <returns>האם הפעולה הצליחה</returns>
        private async Task<bool> MarkAttendanceForDateInternal(DateOnly date, string operationName)
        {
            Console.WriteLine($"{operationName} started for {date}");

            try
            {
                // בדיקה אם התאריך חג
                if (JewishHolidayUtils.IsJewishHoliday(date.ToDateTime(TimeOnly.MinValue)))
                {
                    Console.WriteLine($"התאריך {date} הוא חג/חופש – לא מבצעים נוכחות.");
                    return true;
                }

                var dayName = GetHebrewDayName(date);
                Console.WriteLine($"Day name for the given date: {dayName}");

                var allGroups = dal.Groups.GetGroupsByDayOfWeek(dayName);
                Console.WriteLine($"Total groups found for day '{dayName}': {allGroups.Count()}");

                var groupsWithoutAttendance = allGroups
                    .Where(group => !IsAttendanceMarkedForGroup(group.GroupId, date) &&
                                    !IsGroupCanceledForDay(group.GroupId, date))
                    .ToList();

                if (!groupsWithoutAttendance.Any())
                {
                    Console.WriteLine($"No groups require attendance marking for {date}.");
                    return true;
                }
                // הוסף כאן - הצגת קבוצות שסוננו
                Console.WriteLine($"🔍 Groups after filtering:");
                Console.WriteLine($"   Groups without attendance: {groupsWithoutAttendance.Count}");
                foreach (var g in allGroups.Except(groupsWithoutAttendance))
                {
                    var hasAttendance = IsAttendanceMarkedForGroup(g.GroupId, date);
                    var isCanceled = IsGroupCanceledForDay(g.GroupId, date);
                    Console.WriteLine($"   Group {g.GroupId} FILTERED OUT - HasAttendance: {hasAttendance}, IsCanceled: {isCanceled}");
                }
                int successfulGroups = 0;
                int failedGroups = 0;

                foreach (var group in groupsWithoutAttendance)
                {
                    try
                    {
                        Console.WriteLine($"Processing group {group.GroupId}...");

                        // בדיקה אם תאריך ההתחלה אינו אחרי התאריך
                        if (group.StartDate.HasValue && group.StartDate > date)
                        {
                            Console.WriteLine($"❌ Group {group.GroupId} SKIPPED - has not started yet (StartDate: {group.StartDate} > {date})"); continue;
                        }

                        // בדיקה אם כל השיעורים כבר הושלמו
                        if (group.NumOfLessons.HasValue && group.LessonsCompleted.HasValue &&
                            group.LessonsCompleted >= group.NumOfLessons)
                        {
                            Console.WriteLine($"❌ Group {group.GroupId} SKIPPED - completed all lessons (LessonsCompleted: {group.LessonsCompleted} >= NumOfLessons: {group.NumOfLessons})");
                            continue;
                        }

                        var students = dal.Groups.GetStudentsByGroupId(group.GroupId);
                        Console.WriteLine($"Group {group.GroupId} has {students.Count} students.");

                        var attendanceRecords = students
                            .Where(student => student.IsActive == true &&
                                              student.EnrollmentDate.HasValue &&
                                              student.EnrollmentDate <= date &&
                                              !IsAttendanceMarkedForStudent(student.StudentId, group.GroupId, date))
                            .Select(student => new BLLAttendanceRecord
                            {
                                StudentId = student.StudentId,
                                WasPresent = true
                            })
                            .ToList();
                        Console.WriteLine($"🔍 Student filtering for group {group.GroupId}:");
                        foreach (var student in students)
                        {
                            var isActive = student.IsActive == true;
                            var hasEnrollmentDate = student.EnrollmentDate.HasValue;
                            var enrollmentBeforeDate = student.EnrollmentDate.HasValue && student.EnrollmentDate <= date;
                            var hasNoAttendance = !IsAttendanceMarkedForStudent(student.StudentId, group.GroupId, date);

                            var included = isActive && hasEnrollmentDate && enrollmentBeforeDate && hasNoAttendance;

                            Console.WriteLine($"   Student {student.StudentId}:");
                            Console.WriteLine($"     IsActive: {isActive}");
                            Console.WriteLine($"     HasEnrollmentDate: {hasEnrollmentDate}");
                            Console.WriteLine($"     EnrollmentDate: {student.EnrollmentDate}");
                            Console.WriteLine($"     EnrollmentBeforeDate: {enrollmentBeforeDate}");
                            Console.WriteLine($"     HasNoExistingAttendance: {hasNoAttendance}");
                            Console.WriteLine($"     ➜ INCLUDED: {(included ? "✅ YES" : "❌ NO")}");
                        }

                        Console.WriteLine($"Group {group.GroupId}: {attendanceRecords.Count} attendance records to be marked.");
                        Console.WriteLine($"Group {group.GroupId}: {attendanceRecords.Count} attendance records to be marked.");

                        if (attendanceRecords.Any())
                        {
                            foreach (var record in attendanceRecords)
                            {
                                Console.WriteLine($"Marking attendance for student {record.StudentId} in group {group.GroupId}.");
                            }

                            // שמירת נוכחות - משתמש בפונקציה המשופרת
                            bool attendanceSaved = SaveAttendanceForDateWithLessonCount(group.GroupId, date, attendanceRecords);

                            if (attendanceSaved)
                            {
                                // הוספת תאריכים לא מדווחים
                                foreach (var student in students.Where(s => s.IsActive == true &&
                                                                       s.EnrollmentDate.HasValue &&
                                                                       s.EnrollmentDate <= date))
                                {
                                    Console.WriteLine($"Adding unreported date for student {student.StudentId} in group {group.GroupId}.");
                                    await AddUnreportedDateByAttendanceWithDuplicateCheck(date, student.StudentId);
                                }

                                successfulGroups++;
                                Console.WriteLine($"✅ Successfully processed group {group.GroupId}");
                            }
                            else
                            {
                                failedGroups++;
                                Console.WriteLine($"❌ Failed to save attendance for group {group.GroupId}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No attendance records to mark for group {group.GroupId}.");
                        }
                    }
                    catch (Exception groupEx)
                    {
                        failedGroups++;
                        LogExceptionWithContext(groupEx, $"Processing group {group.GroupId} on {date}",
                            new
                            {
                                groupId = group.GroupId,
                                groupName = group.GroupName,
                                startDate = group.StartDate,
                                numOfLessons = group.NumOfLessons,
                                lessonsCompleted = group.LessonsCompleted,
                               // studentsCount = students?.Count ?? 0,
                               // studentIds = students?.Take(10).Select(s => s.StudentId).ToArray()
                            });
                        Console.WriteLine($"❌ Error processing group {group.GroupId}: {groupEx.Message}");
                    }
                }

                Console.WriteLine($"Attendance marking completed: {successfulGroups} successful, {failedGroups} failed out of {groupsWithoutAttendance.Count} groups.");
                return successfulGroups > 0 || failedGroups == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in {operationName}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// שמירת נוכחות עם עדכון מדויק של מספר השיעורים - מונע כפילויות
        /// </summary>
        private bool SaveAttendanceForDateWithLessonCount(int groupId, DateOnly date, List<BLLAttendanceRecord> attendanceRecords)
        {
            try
            {
                // בדיקה אם כבר יש נוכחות (כדי לדעת אם לעדכן מספר שיעורים)
                bool hadAttendance = IsAttendanceMarkedForGroup(groupId, date);

                // מחיקת נוכחות קיימת
                dal.Attendances.DeleteByGroupAndDate(groupId, date);

                // הוספת נוכחות חדשה
                foreach (var record in attendanceRecords)
                {
                    var student = dal.Students.GetById(record.StudentId);
                    if (student.Status == "פעיל")
                    {
                        dal.Attendances.Create(new Attendance
                        {
                            GroupId = groupId,
                            StudentId = record.StudentId,
                            Date = date,
                            WasPresent = record.WasPresent
                        });
                    }
                }

                // עדכון מספר השיעורים רק אם לא היה שיעור קיים
                if (!hadAttendance)
                {
                    var group = dal.Groups.GetById(groupId);
                    if (group != null)
                    {
                        group.LessonsCompleted = (group.LessonsCompleted ?? 0) + 1;
                        dal.Groups.Update(group);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveAttendanceForDateWithLessonCount: {ex.ToString()}");
                if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbEx && dbEx.Entries != null)
                {
                    foreach (var e in dbEx.Entries)
                    {
                        Console.WriteLine($"Entry type: {e.Entity.GetType().FullName}, State: {e.State}");
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// הוספת תאריך לא מדווח עם בדיקת כפילויות משופרת
        /// </summary>
        private async Task AddUnreportedDateByAttendanceWithDuplicateCheck(DateOnly attendanceDate, int studentId)
        {
            try
            {
                var sth = await dal.StudentHealthFunds.GetAll();
                if (sth == null)
                {
                    Console.WriteLine($"No StudentHealthFunds records at all – skipping student {studentId}");
                    return;
                }

                var hf = sth.FirstOrDefault(x => x.StudentId == studentId);
                if (hf == null)
                {
                    // אין רשימת StudentHealthFund עבור הסטודנט — לא טעות, מדלגים
                    Console.WriteLine($"HealthFundId לא תקין לסטודנט {studentId} — דילוג");
                    return;
                }

                // קבלת תאריכי Unreported רק לאותו StudentHealthFund (מניעת קריאת כל הטבלה)
                var existingUnreportedDates = await dal.UnreportedDates.GetAll();
                if (existingUnreportedDates == null)
                {
                    // אין רשומות כלל בטבלה — נמשיך ליצור
                    existingUnreportedDates = (List<UnreportedDate>?)Enumerable.Empty<UnreportedDate>();
                }


                var alreadyExists = existingUnreportedDates.Any(x =>
                    x.StudentHealthFundId == hf.Id &&
                    x.DateUnreported != null && // Ensure DateUnreported is not null
                    DateOnly.FromDateTime(x.DateUnreported) == attendanceDate); // Directly use x.DateUnreported without '.Value'

                if (!alreadyExists)
                {
                    var unreportedDate = new UnreportedDate
                    {
                        StudentHealthFundId = hf.Id,
                        DateUnreported = attendanceDate.ToDateTime(TimeOnly.MinValue)
                    };

                    await dal.UnreportedDates.Create(unreportedDate);

                    hf.TreatmentsUsed = hf.TreatmentsUsed  + 1;
                    await dal.StudentHealthFunds.Update(hf);
                }
            }
            catch (Exception ex)
            {
                // לוג כללי — לא זורקים הלאה כך שהתהליך הכולל ימשיך
                Console.WriteLine($"שגיאה בזמן ניסיון להוסיף UnreportedDate לסטודנט {studentId}: {ex.Message}");
                // במידת הצורך ניתן להוסיף לוג מפורט יותר פה
            }
        }

        /// <summary>
        /// מחזיר את התאריך הראשון שבו נרשמה נוכחות במערכת - עם error handling מפורט
        /// </summary>
        public DateOnly? GetFirstAttendanceDate()
        {
            try
            {
                Console.WriteLine("🔍 Starting GetFirstAttendanceDate...");

                // בדיקה שה-DAL קיים
                if (dal?.Attendances == null)
                {
                    Console.WriteLine("❌ DAL or Attendances is null");
                    return null;
                }

                Console.WriteLine("✅ DAL.Attendances is available, fetching records...");

                var allAttendances = dal.Attendances.Get();

                Console.WriteLine($"📊 Retrieved {allAttendances?.Count() ?? 0} attendance records from database");

                if (allAttendances == null)
                {
                    Console.WriteLine("❌ allAttendances is null - database returned null");
                    return null;
                }

                if (!allAttendances.Any())
                {
                    Console.WriteLine("❌ No attendance records found in the system.");
                    return null;
                }

                Console.WriteLine("🔍 Analyzing attendance records for dates...");

                var recordsWithDates = allAttendances.Where(a => a.Date.HasValue).ToList();

                Console.WriteLine($"📊 Found {recordsWithDates.Count} records with valid dates out of {allAttendances.Count()} total records");

                if (!recordsWithDates.Any())
                {
                    Console.WriteLine("❌ No attendance records have valid dates");
                    return null;
                }

                var firstDate = recordsWithDates.Min(a => a.Date.Value);

                Console.WriteLine($"✅ First attendance date found: {firstDate}");
                Console.WriteLine($"📊 Date range: {firstDate} to {recordsWithDates.Max(a => a.Date.Value)}");

                return firstDate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Critical error in GetFirstAttendanceDate:");
                Console.WriteLine($"   Message: {ex.Message}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");

                // בדיקה אם זו בעיית חיבור למסד נתונים
                if (ex.Message.Contains("connection") || ex.Message.Contains("timeout"))
                {
                    Console.WriteLine("💡 This appears to be a database connection issue");
                }

                // בדיקה אם זו בעיית הרשאות
                if (ex.Message.Contains("permission") || ex.Message.Contains("access"))
                {
                    Console.WriteLine("💡 This appears to be a database permissions issue");
                }

                return null;
            }
        }

        /// <summary>
        /// סימון נוכחות היסטורי - משתמש בפונקציה המרכזית
        /// </summary>
        public async Task<bool> MarkHistoricalAttendance(DateOnly startDate, DateOnly? endDate = null)
        {
            var actualEndDate = endDate ?? DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine($"MarkHistoricalAttendance started from {startDate} to {actualEndDate}");

            if (startDate > actualEndDate)
            {
                Console.WriteLine("Error: Start date is after end date.");
                return false;
            }

            int totalDays = 0;
            int processedDays = 0;
            int skippedDays = 0;
            int errorDays = 0;

            try
            {
                for (var currentDate = startDate; currentDate <= actualEndDate; currentDate = currentDate.AddDays(1))
                {
                    totalDays++;

                    try
                    {
                        // בדיקה מהירה אם כבר יש נוכחות ליום הזה
                        var dayName = GetHebrewDayName(currentDate);
                        var allGroups = dal.Groups.GetGroupsByDayOfWeek(dayName);

                        if (!allGroups.Any())
                        {
                            Console.WriteLine($"Skipping {currentDate} ({dayName}) - No groups for this day");
                            skippedDays++;
                            continue;
                        }

                        // שינוי חשוב: דולג רק אם כל הקבוצות כבר מסומנות (All),
                        // ולא כפי שהיה קודם - שדלג אם יש אפילו קבוצה אחת (Any).
                        bool allGroupsHaveAttendance = allGroups.All(g => IsAttendanceMarkedForGroup(g.GroupId, currentDate));

                        if (allGroupsHaveAttendance)
                        {
                            Console.WriteLine($"Skipping {currentDate} - All groups already have attendance records");
                            skippedDays++;
                            continue;
                        }

                        
                         await MarkAttendanceForDate(currentDate);

                       
                            processedDays++;
                            Console.WriteLine($"✅ Successfully processed {currentDate}");
                       
                       
                    }
                    catch (Exception dayEx)
                    {
                        errorDays++;
                        Console.WriteLine($"❌ Error processing {currentDate}: {dayEx.Message}");
                    }

                    // הפסקה קטנה כל 10 ימים
                    if (totalDays % 10 == 0)
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"Progress: {totalDays} days processed...");
                    }
                }

                Console.WriteLine($"Historical attendance marking completed:");
                Console.WriteLine($"Total days: {totalDays}, Processed: {processedDays}, Skipped: {skippedDays}, Errors: {errorDays}");

                return errorDays == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in MarkHistoricalAttendance: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// סימון נוכחות אוטומטי יומי
        /// </summary>
        public async Task AutoMarkDailyAttendance()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            bool result = await MarkAttendanceForDateInternal(today, "AutoMarkDailyAttendance");

            if (result)
            {
                Console.WriteLine($"Successfully marked attendance for {today}.");
            }
            else
            {
                Console.WriteLine($"Error in AutoMarkDailyAttendance for {today}.");
            }
        }
        /// <summary>
        /// סימון נוכחות עבור תאריך מסוים
        /// </summary>
        public async Task MarkAttendanceForDate(DateOnly date)
        {
            await MarkAttendanceForDateInternal(date, "MarkAttendanceForDate");
        }

        private void LogExceptionWithContext(Exception ex, string context, object? meta = null)
        {
            Console.WriteLine($"[{DateTime.UtcNow:O}] ERROR in {context}");
            Console.WriteLine(ex.ToString()); // includes InnerException and StackTrace

            if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbEx && dbEx.Entries != null)
            {
                foreach (var entry in dbEx.Entries)
                {
                    try
                    {
                        var type = entry.Entity?.GetType().FullName ?? "Unknown";
                        Console.WriteLine($"   EF Entry Type: {type}, State: {entry.State}");
                        // attempt to print properties
                        var props = entry.CurrentValues;
                        if (props != null)
                        {
                            foreach (var propName in props.Properties.Select(p => p.Name))
                            {
                                Console.WriteLine($"      {propName}: {props[propName]}");
                            }
                        }
                    }
                    catch (Exception inner)
                    {
                        Console.WriteLine($"   Failed to read EF entry properties: {inner.Message}");
                    }
                }
            }

            if (meta != null)
            {
                try
                {
                    var metaJson = System.Text.Json.JsonSerializer.Serialize(meta);
                    Console.WriteLine($"   Context meta: {metaJson}");
                }
                catch
                {
                    Console.WriteLine("   (Could not serialize meta)");
                }
            }
        }


    }
}
