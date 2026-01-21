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

                return lessons.Select(l => new BLLLesson
                {
                    LessonId = l.LessonId,
                    GroupId = l.GroupId,
                    LessonDate = l.LessonDate,
                    LessonHour = l.LessonHour,
                    InstructorId = l.InstructorId,
                    Status = l.Status,
                    IsReported = l.IsReported,
                    CreatedAt = l.CreatedAt,
                    CreatedBy = l.CreatedBy
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lessons: {ex.Message}");
                return new List<BLLLesson>();
            }
        }

        public async Task Create(BLLLesson lesson)
        {
            var l = new Lesson
            {
                GroupId = lesson.GroupId,
                LessonDate = lesson.LessonDate,
                LessonHour = lesson.LessonHour,
                InstructorId = lesson.InstructorId,
                Status = lesson.Status,
                IsReported = lesson.IsReported,
                CreatedAt = lesson.CreatedAt,
                CreatedBy = lesson.CreatedBy
            };
             dal.Lessons.Create(l);
        }

        public BLLLesson GetById(int id)
        {
            try
            {
                var l = dal.Lessons.GetById(id);
                if (l != null)
                {
                    return new BLLLesson
                    {
                        LessonId = l.LessonId,
                        GroupId = l.GroupId,
                        LessonDate = l.LessonDate,
                        LessonHour = l.LessonHour,
                        InstructorId = l.InstructorId,
                        Status = l.Status,
                        IsReported = l.IsReported,
                        CreatedAt = l.CreatedAt,
                        CreatedBy = l.CreatedBy
                    };
                }
                Console.WriteLine($"Lesson with ID {id} not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lesson with ID {id}: {ex.Message}");
                return null;
            }
        }

        public void Delete(int id)
        {
            dal.Lessons.Delete(id);
        }

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
                        string status = currentDate.Date < DateTime.Today ? "completed" :
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
                return lessons.Select(l => new BLLLesson
                {
                    LessonId = l.LessonId,
                    GroupId = l.GroupId,
                    LessonDate = l.LessonDate,
                    LessonHour = l.LessonHour,
                    InstructorId = l.InstructorId,
                    Status = l.Status,
                    IsReported = l.IsReported,
                    CreatedAt = l.CreatedAt,
                    CreatedBy = l.CreatedBy
                }).OrderBy(l => l.LessonDate).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בשליפת שיעורים עבור קבוצה {groupId}: {ex.Message}");
                return new List<BLLLesson>();
            }
        }
    }
}
