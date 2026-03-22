using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLLessonService : IBLLLesson
    {
        private readonly IDAL dal;

        public BLLLessonService(IDAL dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// המרה מ BLL ל DAL
        /// </summary>
        /// <param name="bllLesson"></param>
        /// <returns></returns>
        private Lesson ToLesson(BLLLesson bllLesson)
        {
            return new Lesson
            {
                LessonId = bllLesson.LessonId,
                GroupId = bllLesson.GroupId,
                LessonDate = bllLesson.LessonDate,
                LessonHour = bllLesson.LessonHour,
                InstructorId = bllLesson.InstructorId,
                Status = bllLesson.Status,
                CancellationReason = bllLesson.CancellationReason,
                CanceledAt = bllLesson.CanceledAt,
                CanceledBy = bllLesson.CanceledBy,
                IsReported = bllLesson.IsReported,
                CreatedAt = bllLesson.CreatedAt,
                CreatedBy = bllLesson.CreatedBy
            };
        }

        /// <summary>
        /// המרה מ DAL ל BLL
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        private BLLLesson ToBLLLesson(Lesson lesson)
        {
            return new BLLLesson
            {
                LessonId = lesson.LessonId,
                GroupId = lesson.GroupId,
                LessonDate = lesson.LessonDate,
                LessonHour = lesson.LessonHour,
                InstructorId = lesson.InstructorId,
                Status = lesson.Status,
                CancellationReason = lesson.CancellationReason,
                CanceledAt = lesson.CanceledAt,
                CanceledBy = lesson.CanceledBy,
                IsReported = lesson.IsReported,
                CreatedAt = lesson.CreatedAt,
                CreatedBy = lesson.CreatedBy
            };
        }

        /// <summary>
        /// החזרת כל השיעורים
        /// </summary>
        /// <returns></returns>
        public List<BLLLesson> Get()
        {
            try
            {
                var lessons = dal.Lessons.Get();
                if (lessons == null || !lessons.Any())
                {
                    Console.WriteLine("No lessons found.");
                    return new List<BLLLesson>();
                }

                return lessons.Select(ToBLLLesson).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lessons: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// יצירת שיעור
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        public async Task Create(BLLLesson lesson)
        {
            var l = ToLesson(lesson);
            await Task.Run(() => dal.Lessons.Create(l));
        }

        /// <summary>
        /// החזרת שיעור לפי קוד שיעור
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BLLLesson GetById(int id)
        {
            var l = dal.Lessons.GetById(id);
            if (l == null)
            {
                throw new KeyNotFoundException($"Lesson with ID {id} not found.");
            }
            return ToBLLLesson(l);
        }

        /// <summary>
        /// מחיקת שיעור
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            dal.Lessons.Delete(id);
        }

        /// <summary>
        /// עדכון פרטי שיעור
        /// </summary>
        /// <param name="lesson"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Update(BLLLesson lesson)
        {
            var l = dal.Lessons.GetById(lesson.LessonId);
            if (l == null)
                throw new KeyNotFoundException($"Lesson with ID {lesson.LessonId} not found.");

            l.GroupId = lesson.GroupId;
            l.LessonDate = lesson.LessonDate;
            l.LessonHour = lesson.LessonHour;
            l.InstructorId = lesson.InstructorId;
            l.Status = lesson.Status;
            l.IsReported = lesson.IsReported;
            l.CreatedAt = lesson.CreatedAt;
            l.CreatedBy = lesson.CreatedBy;

            dal.Lessons.Update(l);
        }

        /// <summary>
        /// פונקציה ליצירת שיעורים אוטומטית עבור קבוצה  
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="startDate"></param>
        /// <param name="numOfLessons"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="lessonHour"></param>
        /// <param name="instructorId"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task GenerateLessonsForGroup(int groupId, DateOnly startDate, int numOfLessons, string dayOfWeek, TimeOnly lessonHour, int instructorId, string createdBy)
        {
            await Task.Run(() =>
            {
                try
                {
                    // המרת יום בשבוע מעברית לאנגלית
                    var dayMapping = new Dictionary<string, DayOfWeek>
                    {
                        { "ראשון", DayOfWeek.Sunday },
                        { "שני", DayOfWeek.Monday },
                        { "שלישי", DayOfWeek.Tuesday },
                        { "רביעי", DayOfWeek.Wednesday },
                        { "חמישי", DayOfWeek.Thursday },
                        { "שישי", DayOfWeek.Friday },
                        { "שבת", DayOfWeek.Saturday }
                    };

                    if (!dayMapping.TryGetValue(dayOfWeek, out DayOfWeek targetDay))
                    {
                        throw new ArgumentException($"יום בשבוע לא תקין: {dayOfWeek}");
                    }

                    // מציאת היום הראשון שמתאים ליום בשבוע מתאריך ההתחלה
                    DateTime currentDate = startDate.ToDateTime(TimeOnly.MinValue);

                    // אם תאריך ההתחלה לא מתאים ליום בשבוע, מצא את היום הקרוב ביותר
                    while (currentDate.DayOfWeek != targetDay)
                    {
                        currentDate = currentDate.AddDays(1);
                    }

                    var lessonsToCreate = new List<Lesson>();
                    int createdLessonsCount = 0;
                    int maxIterations = numOfLessons * 4; // הגנה מפני לולאה אינסופית
                    int iterations = 0;

                    while (createdLessonsCount < numOfLessons && iterations < maxIterations)
                    {
                        iterations++;

                        bool isHoliday = false;
                        if (currentDate.Ticks >= 49916304000000000 && currentDate.Ticks <= 68277647999999999)
                        {
                            // רק אז לקרוא ל-IsJewishHoliday
                            isHoliday = JewishHolidayUtils.IsJewishHoliday(currentDate);
                        }
                        else
                        {
                            // טיפול בשגיאה או דילוג
                            isHoliday = false; // או כל טיפול אחר
                        }

                        if (!isHoliday)
                        {
                            // קביעת סטטוס השיעור
                            string status = currentDate.Date < DateTime.Today ? "done" :
                currentDate.Date == DateTime.Today ? "today" : "future";

                            var lesson = new Lesson
                            {
                                GroupId = groupId,
                                LessonDate = DateOnly.FromDateTime(currentDate),
                                LessonHour = lessonHour,
                                InstructorId = instructorId,
                                Status = status,
                                IsReported = false,
                                CreatedAt = DateTime.Now,
                                CreatedBy = createdBy
                            };

                            lessonsToCreate.Add(lesson);
                            createdLessonsCount++;

                            Console.WriteLine($"נוצר שיעור #{createdLessonsCount} בתאריך {currentDate:yyyy-MM-dd}");
                        }
                        else
                        {
                            Console.WriteLine($"דולג על תאריך {currentDate:yyyy-MM-dd} - חג/חופש");
                        }

                        // מעבר לשבוע הבא (7 ימים)
                        currentDate = currentDate.AddDays(7);
                    }

                    // יצירת כל השיעורים בבסיס הנתונים
                    foreach (var lesson in lessonsToCreate)
                    {
                        dal.Lessons.Create(lesson);
                    }

                    Console.WriteLine($"נוצרו {createdLessonsCount} שיעורים עבור קבוצה {groupId} (דולג על {iterations - createdLessonsCount} חגים)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"שגיאה ביצירת שיעורים עבור קבוצה {groupId}: {ex.Message}");
                    throw;
                }
            });
        }

        /// <summary>
        /// פונקציה לקבלת כל השיעורים של קבוצה ספציפית
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<BLLLesson> GetByGroupId(int groupId)
        {
            try
            {
                var lessons = dal.Lessons.Get().Where(l => l.GroupId == groupId).ToList();
                var now = DateOnly.FromDateTime(DateTime.Now); // Convert DateTime to DateOnly

                return lessons.Select(l =>
                {
                    var status = l.Status;

                    if (status != "canceled" && status != "completion")
                    {
                        if (l.LessonDate < now)
                            status = "done";
                        else if (l.LessonDate > now)
                            status = "future";
                    }

                    return new BLLLesson
                    {
                        LessonId = l.LessonId,
                        GroupId = l.GroupId,
                        LessonDate = l.LessonDate,
                        LessonHour = l.LessonHour,
                        InstructorId = l.InstructorId,
                        Status = status,
                        IsReported = l.IsReported,
                        CreatedAt = l.CreatedAt,
                        CreatedBy = l.CreatedBy
                    };
                })
                .OrderBy(l => l.LessonDate)
                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורים עבור קבוצה {groupId}: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// החזרת שיעורים שמתקיימים בתאריך מסוים
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<(int LessonId, string GroupName, string LessonStatus, string City)> GetGroupsWithLessonsByDate(DateOnly date)
        {
            var lessonsOnDate = dal.Lessons.Get()
                .Where(l => l.LessonDate == date)
                .ToList();

            var groupIds = lessonsOnDate.Select(l => l.GroupId).Distinct().ToList();
            var groups = dal.Groups.Get().Where(g => groupIds.Contains(g.GroupId)).ToList();

            // שליפת כל הסניפים הרלוונטיים מראש
            var branchIds = groups.Select(g => g.BranchId).Distinct().ToList();
            var branches = branchIds.ToDictionary(
                id => id,
                id => dal.Branches.GetById(id)?.City ?? string.Empty
            );

            var result = lessonsOnDate
                .Join(groups,
                      lesson => lesson.GroupId,
                      group => group.GroupId,
                      (lesson, group) => (
                          LessonId: lesson.LessonId,
                          GroupName: group.GroupName,
                          LessonStatus: lesson.Status,
                          City: branches.ContainsKey(group.BranchId) ? branches[group.BranchId] : string.Empty
                      ))
                .ToList();

            return result;
        }

        /// <summary>
        /// ביטול שיעור ספציפי
        /// </summary>
        public void CancelLesson(int lessonId, string reason, string canceledBy)
        {
            var lesson = dal.Lessons.GetById(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException($"Lesson with ID {lessonId} not found.");

            lesson.Status = "canceled";
            lesson.CancellationReason = reason;
            lesson.CanceledAt = DateTime.Now;
            lesson.CanceledBy = canceledBy;

            dal.Lessons.Update(lesson);
        }

        /// <summary>
        /// ביטול כל השיעורים של קבוצות ביום מסוים
        /// </summary>
        public void CancelAllGroupsForDay(string dayOfWeek, DateOnly date,
            string reason, string createdBy)
        {
            try
            {
                var allLessons = dal.Lessons.Get();
                var allGroups = dal.Groups.Get();

                var lessonsToCancelQuery = allLessons
                    .Where(l => l.LessonDate == date && l.Status != "canceled")
                    .Join(allGroups,
                          lesson => lesson.GroupId,
                          group => group.GroupId,
                          (lesson, group) => new { lesson, group })
                    .Where(x => x.group.DayOfWeek == dayOfWeek)
                    .Select(x => x.lesson)
                    .ToList();

                foreach (var lesson in lessonsToCancelQuery)
                {
                    lesson.Status = "canceled";
                    lesson.CancellationReason = reason;
                    lesson.CanceledAt = DateTime.Now;
                    lesson.CanceledBy = createdBy;
                    dal.Lessons.Update(lesson);
                }

                Console.WriteLine($"ביטלו {lessonsToCancelQuery.Count} שיעורים ליום {date}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בביטול שיעורים ליום {date}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// קבלת כל השיעורים שבוטלו בתאריך מסוים
        /// </summary>
        public List<BLLLesson> GetCanceledLessonsByDate(DateOnly date)
        {
            try
            {
                var canceledLessons = dal.Lessons.Get()
                    .Where(l => l.LessonDate == date && l.Status == "canceled")
                    .ToList();

                return canceledLessons.Select(ToBLLLesson).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורים בוטלים: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// ביטול ביטול (החזרת שיעור לסטטוס קודם)
        /// </summary>
        public void UndoCancelLesson(int lessonId, string undoBy)
        {
            var lesson = dal.Lessons.GetById(lessonId);
            if (lesson == null)
                throw new KeyNotFoundException($"Lesson with ID {lessonId} not found.");

            if (lesson.Status != "canceled")
                throw new InvalidOperationException($"Lesson {lessonId} is not canceled.");

            var now = DateOnly.FromDateTime(DateTime.Now);
            lesson.Status = lesson.LessonDate < now ? "done" :
                            lesson.LessonDate == now ? "today" : "future"; lesson.CancellationReason = null;
            lesson.CanceledAt = null;
            lesson.CanceledBy = null;

            dal.Lessons.Update(lesson);
        }

        /// <summary>
        /// קבלת כל השיעורים המבוטלים
        /// </summary>
        public List<BLLLesson> GetCanceledLessons()
        {
            try
            {
                var canceledLessons = dal.Lessons.GetCanceledLessons();
                return canceledLessons.Select(ToBLLLesson).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורים בוטלים: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// יצירת שיעור השלמה
        /// </summary>
        public async Task CreateCompletionLesson(int groupId, DateOnly completionDate,
     TimeOnly completionHour, int instructorId, string createdBy)
        {
            try
            {
                // אם לא נשלח קוד מדריך, נשלוף אותו מהקבוצה
                if (instructorId == 0)
                {
                    var group = dal.Groups.GetById(groupId);
                    if (group == null)
                        throw new ArgumentException($"Group with ID {groupId} not found.");
                    instructorId = group.InstructorId;
                }

                var completionLesson = new Lesson
                {
                    GroupId = groupId,
                    LessonDate = completionDate,
                    LessonHour = completionHour,
                    InstructorId = instructorId,
                    Status = "completion",
                    IsReported = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                    CancellationReason = null,
                    CanceledAt = null,
                    CanceledBy = null
                };

                // אחרי יצירת שיעור השלמה:
                await Task.Run(() => dal.Lessons.Create(completionLesson));

                // הוספת נוכחות לכל תלמידי הקבוצה עבור שיעור ההשלמה
                var students = dal.Groups.GetStudentsByGroupId(groupId);
                foreach (var student in students)
                {
                    var studentDetails = dal.Students.GetById(student.StudentId);

                    var attendance = new Attendance
                    {
                        LessonId = completionLesson.LessonId,
                        StudentId = student.StudentId,
                        WasPresent = false, // ברירת מחדל – לא סומן
                        StatusReport = 3,   // ממתין לדיווח לפי הצורך
                        UpdateDate = DateTime.Now,
                        UpdateBy = null,   
                        HealthFundReport = studentDetails.HealthFundId, // קופת החולים של התלמיד
                        DateReport = null   
                    };
                    dal.Attendances.Create(attendance);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה ביצירת שיעור השלמה: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// שינוי סטטוס של שיעור להשלמה
        /// </summary>
        public void MarkLessonAsCompletion(int lessonId, string markedBy)
        {
            try
            {
                var lesson = dal.Lessons.GetById(lessonId);
                if (lesson == null)
                    throw new KeyNotFoundException($"Lesson with ID {lessonId} not found.");

                lesson.Status = "completion";
                lesson.CreatedBy = markedBy;

                dal.Lessons.Update(lesson);
                Console.WriteLine($"שיעור {lessonId} סומן כהשלמה על ידי {markedBy}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בסימון שיעור כהשלמה: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// קבלת כל שיעורי ההשלמה
        /// </summary>
        public List<BLLLesson> GetCompletionLessons()
        {
            try
            {
                var completionLessons = dal.Lessons.GetCompletionLessons();
                return completionLessons.Select(ToBLLLesson).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורי השלמה: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// קבלת כל שיעורי ההשלמה של קבוצה ספציפית
        /// </summary>
        public List<BLLLesson> GetCompletionLessonsByGroupId(int groupId)
        {
            try
            {
                var completionLessons = dal.Lessons.GetCompletionLessons()
                    .Where(l => l.GroupId == groupId)
                    .ToList();
                return completionLessons.Select(ToBLLLesson).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורי השלמה לקבוצה {groupId}: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        /// <summary>
        /// נרמול סטטוס
        /// </summary>
        /// <param name="lesson"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        private static string NormalizeStatus(Lesson lesson, DateOnly today)
        {
            if (lesson.Status == "canceled" || lesson.Status == "completion")
                return lesson.Status;

            if (lesson.LessonDate < today) return "done";
            if (lesson.LessonDate == today) return "today";
            return "future";
        }
      
        /// <summary>
        /// קבלת שיעורים לתאריך מסוים
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<LessonCalendarItemDto> GetLessonsForCalendarByDate(DateOnly date)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var lessonsOnDate = dal.Lessons.Get()
                    .Where(l => l.LessonDate == date)
                    .ToList();

                if (!lessonsOnDate.Any())
                    return new List<LessonCalendarItemDto>();

                var groupIds = lessonsOnDate.Select(l => l.GroupId).Distinct().ToList();
                var groups = dal.Groups.Get().Where(g => groupIds.Contains(g.GroupId)).ToList();

                var branchIds = groups.Select(g => g.BranchId).Distinct().ToList();
                var branches = dal.Branches.Get().Where(b => branchIds.Contains(b.BranchId))
                    .ToDictionary(b => b.BranchId, b => b);

                var courseIds = groups.Select(g => g.CourseId).Distinct().ToList();
                var courses = dal.Courses.Get().Where(c => courseIds.Contains(c.CourseId))
                    .ToDictionary(c => c.CourseId, c => c);

                var result = lessonsOnDate
                    .Join(groups, l => l.GroupId, g => g.GroupId, (l, g) => new { l, g })
                    .Select(x =>
                    {
                        branches.TryGetValue(x.g.BranchId, out var branch);
                        courses.TryGetValue(x.g.CourseId, out var course);

                        return new LessonCalendarItemDto
                        {
                            LessonId = x.l.LessonId,
                            GroupId = x.g.GroupId,
                            GroupName = x.g.GroupName,
                            BranchId = x.g.BranchId,
                            BranchName = branch?.City ?? string.Empty,
                            CourseId = x.g.CourseId,
                            CourseName = course?.CouresName ?? string.Empty, // לפי שם השדה אצלך
                            LessonDate = x.l.LessonDate,
                            LessonHour = x.l.LessonHour,
                            InstructorId = x.l.InstructorId,
                            LessonStatus = NormalizeStatus(x.l, today),
                            CancellationReason = x.l.CancellationReason
                        };
                    })
                    .OrderBy(x => x.LessonHour)
                    .ThenBy(x => x.GroupName)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLessonsForCalendarByDate: {ex.Message}");
                return new List<LessonCalendarItemDto>();
            }
        }

        /// <summary>
        ///  קבלת שיעורים לפי טווח תאריכים
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<LessonCalendarItemDto> GetLessonsForCalendarByDateRange(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

                var lessons = dal.Lessons.Get()
                    .Where(l => l.LessonDate >= startDate && l.LessonDate <= endDate)
                    .ToList();

                if (!lessons.Any())
                    return new List<LessonCalendarItemDto>();

                var groupIds = lessons.Select(l => l.GroupId).Distinct().ToList();
                var groups = dal.Groups.Get().Where(g => groupIds.Contains(g.GroupId)).ToList();

                var branchIds = groups.Select(g => g.BranchId).Distinct().ToList();
                var branches = dal.Branches.Get().Where(b => branchIds.Contains(b.BranchId))
                    .ToDictionary(b => b.BranchId, b => b);

                var courseIds = groups.Select(g => g.CourseId).Distinct().ToList();
                var courses = dal.Courses.Get().Where(c => courseIds.Contains(c.CourseId))
                    .ToDictionary(c => c.CourseId, c => c);

                return lessons
                    .Join(groups, l => l.GroupId, g => g.GroupId, (l, g) => new { l, g })
                    .Select(x =>
                    {
                        branches.TryGetValue(x.g.BranchId, out var branch);
                        courses.TryGetValue(x.g.CourseId, out var course);

                        return new LessonCalendarItemDto
                        {
                            LessonId = x.l.LessonId,
                            GroupId = x.g.GroupId,
                            GroupName = x.g.GroupName,
                            BranchId = x.g.BranchId,
                            BranchName = branch?.City ?? string.Empty,
                            CourseId = x.g.CourseId,
                            CourseName = course?.CouresName ?? string.Empty,
                            LessonDate = x.l.LessonDate,
                            LessonHour = x.l.LessonHour,
                            InstructorId = x.l.InstructorId,
                            LessonStatus = NormalizeStatus(x.l, today),
                            CancellationReason = x.l.CancellationReason
                        };
                    })
                    .OrderBy(x => x.LessonDate)
                    .ThenBy(x => x.LessonHour)
                    .ThenBy(x => x.GroupName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLessonsForCalendarByDateRange: {ex.Message}");
                return new List<LessonCalendarItemDto>();
            }
        }
    }
}
