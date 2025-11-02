using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            dal.Attendances.Delete(attendanceId);
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
            catch
            {
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

                var groups = dal.Groups.GetGroupsByDayOfWeek(dayName);

                // בדיקה אם יש קבוצות בכלל
                if (groups == null || !groups.Any())
                {
                    Console.WriteLine($"No groups found for date {date}");
                    return true;
                }

                // בדיקה עבור כל קבוצה
                foreach (var item in groups)
                {
                    if (IsAttendanceMarkedForGroup(item.GroupId, date) == false)
                    {
                        Console.WriteLine($"Attendance not marked for group {item.GroupId} on {date}");
                        return false;
                    }
                }

                Console.WriteLine($"Attendance marked for all {groups.Count()} groups on {date}");
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

        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
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
            var sth = await dal.StudentHealthFunds.GetAll();
           

            var hf = sth.FirstOrDefault(x => x.StudentId == studentId);
            if (hf != null)
            {
                var unreportedDate = new UnreportedDate
                {
                    StudentHealthFundId = hf.HealthFundId,
                    DateUnreported = attendanceDate.ToDateTime(TimeOnly.MinValue)
                };

                await dal.UnreportedDates.Create(unreportedDate);
              
                // עדכון TreatmentsUsed במספר הרשומות שנוצרו
                hf.TreatmentsUsed = hf.TreatmentsUsed++;
                await dal.StudentHealthFunds.Update(hf);
            }

         

        }


        /// <summary>
        /// סימון נוכחות אוטומטי
        /// </summary>
        public async Task AutoMarkDailyAttendance()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine($"AutoMarkDailyAttendance started for {today}");

            try
            {
                // בדיקה אם היום חג
                if (JewishHolidayUtils.IsJewishHoliday(today.ToDateTime(TimeOnly.MinValue)))
                {
                    Console.WriteLine($"היום {today} הוא חג/חופש – לא מבצעים נוכחות.");
                    return;
                }

                var dayName = GetHebrewDayName(today);
                Console.WriteLine($"Day name for today: {dayName}");

                var groupsWithoutAttendance = dal.Groups.GetGroupsByDayOfWeek(dayName)
                    .Where(group => !IsAttendanceMarkedForGroup(group.GroupId, today) &&
                                    !IsGroupCanceledForDay(group.GroupId, today))
                    .ToList();

                if (!groupsWithoutAttendance.Any())
                {
                    Console.WriteLine($"No groups require attendance marking for {today}.");
                    return;
                }

                foreach (var group in groupsWithoutAttendance)
                {
                    // בדיקה אם תאריך ההתחלה אינו אחרי התאריך הנוכחי
                    if (group.StartDate.HasValue && group.StartDate > today)
                    {
                        Console.WriteLine($"Group {group.GroupId} has not started yet (StartDate: {group.StartDate}). Skipping.");
                        continue;
                    }

                    // בדיקה אם כל השיעורים כבר הושלמו
                    if (group.NumOfLessons.HasValue && group.LessonsCompleted.HasValue &&
                        group.LessonsCompleted >= group.NumOfLessons)
                    {
                        Console.WriteLine($"Group {group.GroupId} has completed all lessons (LessonsCompleted: {group.LessonsCompleted}, NumOfLessons: {group.NumOfLessons}). Skipping.");
                        continue;
                    }

                    var students = dal.Groups.GetStudentsByGroupId(group.GroupId);

                    var attendanceRecords = students
     .Where(student => student.IsActive == true && student.EnrollmentDate <= today &&
                       !IsAttendanceMarkedForStudent(student.StudentId, group.GroupId, today) &&
                       student.EnrollmentDate.HasValue && student.EnrollmentDate <= today)
     .Select(student => new BLLAttendanceRecord
     {
         StudentId = student.StudentId,
         WasPresent = true
     })
     .ToList();


                    if (attendanceRecords.Any())
                    {
                        SaveAttendanceForDate(group.GroupId, today, attendanceRecords);
                    }
                    foreach (var item in students)
                    {
                        await AddUnreportedDateByAttendance(today, item.StudentId);
                    }

                }

                Console.WriteLine($"Successfully marked attendance for {groupsWithoutAttendance.Count} groups on {today}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AutoMarkDailyAttendance: {ex.Message}");
            }
        }
        /// <summary>
        /// סימון נוכחות עבור תאריך מסוים
        /// </summary>
        public async Task  MarkAttendanceForDate(DateOnly date)
        {
            Console.WriteLine($"MarkAttendanceForDate started for {date}");

            try
            {
                // בדיקה אם התאריך חג
                if (JewishHolidayUtils.IsJewishHoliday(date.ToDateTime(TimeOnly.MinValue)))
                {
                    Console.WriteLine($"התאריך {date} הוא חג/חופש – לא מבצעים נוכחות.");
                    return;
                }

                var dayName = GetHebrewDayName(date);
                Console.WriteLine($"Day name for the given date: {dayName}");

                var groupsWithoutAttendance = dal.Groups.GetGroupsByDayOfWeek(dayName)
                    .Where(group => !IsAttendanceMarkedForGroup(group.GroupId, date) &&
                                    !IsGroupCanceledForDay(group.GroupId, date))
                    .ToList();

                if (!groupsWithoutAttendance.Any())
                {
                    Console.WriteLine($"No groups require attendance marking for {date}.");
                    return;
                }

                foreach (var group in groupsWithoutAttendance)
                {
                    // בדיקה אם תאריך ההתחלה אינו אחרי התאריך
                    if (group.StartDate.HasValue && group.StartDate > date)
                    {
                        Console.WriteLine($"Group {group.GroupId} has not started yet (StartDate: {group.StartDate}). Skipping.");
                        continue;
                    }

                    // בדיקה אם כל השיעורים כבר הושלמו
                    if (group.NumOfLessons.HasValue && group.LessonsCompleted.HasValue &&
                        group.LessonsCompleted >= group.NumOfLessons)
                    {
                        Console.WriteLine($"Group {group.GroupId} has completed all lessons (LessonsCompleted: {group.LessonsCompleted}, NumOfLessons: {group.NumOfLessons}). Skipping.");
                        continue;
                    }

                    var students = dal.Groups.GetStudentsByGroupId(group.GroupId);

                    var attendanceRecords = students
     .Where(student => student.IsActive == true && student.EnrollmentDate <= date &&
                       !IsAttendanceMarkedForStudent(student.StudentId, group.GroupId, date))
     .Select(student => new BLLAttendanceRecord
     {
         StudentId = student.StudentId,
         WasPresent = true
     })
     .ToList();


                    if (attendanceRecords.Any())
                    {
                        SaveAttendanceForDate(group.GroupId, date, attendanceRecords);
                      
                    }
                    foreach (var item in students)
                    {
                      await  AddUnreportedDateByAttendance(date, item.StudentId);
                    }

                }

                Console.WriteLine($"Successfully marked attendance for {groupsWithoutAttendance.Count} groups on {date}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkAttendanceForDate: {ex.Message}");
            }
        }
        






    }
}
