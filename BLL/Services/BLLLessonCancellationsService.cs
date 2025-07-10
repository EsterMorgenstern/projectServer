using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLLessonCancellationsService : IBLLLessonCancellations
    {
        private readonly IDAL dal;

        public BLLLessonCancellationsService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLLessonCancellations lc)
        {
            LessonCancellations lcn = new LessonCancellations()
            {
                Id = lc.Id,
                GroupId = lc.GroupId,
                Reason = lc.Reason,
                Date = lc.Date,
                Created_at = lc.Created_at,
                Created_by = lc.Created_by
            };
            dal.LessonCancellations.Create(lcn);
        }
        /// <summary>
        /// ביטול שיעורים לכל הקבוצות ביום מסוים
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <param name="date"></param>
        /// <param name="reason"></param>
        /// <param name="createdBy"></param>

        public void CancelAllGroupsForDay(string dayOfWeek, DateTime date, string reason, string createdBy)
        {
            DateOnly dateOnly = DateOnly.FromDateTime(date);
            DateOnly createdAtOnly = DateOnly.FromDateTime(DateTime.Now);

            dal.LessonCancellations.CancelAllGroupsForDay(dayOfWeek, dateOnly, reason, createdBy, createdAtOnly);
        }
        /// <summary>
        /// קבלת כל הביטולים לתאריך מסוים
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>

        public List<BLLLessonCancellations> GetCancellationsByDate(DateTime date)
        {
            var dalCancellations = dal.LessonCancellations.GetCancellationsByDate(date);

            return dalCancellations.Select(c => new BLLLessonCancellations()
            {
                Id = c.Id,
                GroupId = c.GroupId,
                Created_at = c.Created_at,
                Created_by = c.Created_by,
                Date = c.Date,
                Reason = c.Reason
            }).ToList();
        }
        /// <summary>
        /// ביטול כל הביטולים ליום מסוים
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <param name="date"></param>

        public void RemoveAllCancellationsForDay(string dayOfWeek, DateTime date)
        {
            dal.LessonCancellations.RemoveAllCancellationsForDay(dayOfWeek, date);
        }
        /// <summary>
        /// קבלת פרטי ביטולים מורחבים לתאריך מסוים (עם שמות קבוצות, קורסים וסניפים)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>

        public List<BLLLessonCancellationsDetails> GetCancellationDetailsByDate(DateTime date)
        {
            var cancellations = dal.LessonCancellations.GetCancellationsByDate(date);

            return cancellations.Select(c => new BLLLessonCancellationsDetails()
            {
                Id = c.Id,
                GroupId = c.GroupId,
                GroupName = dal.Groups.GetById(c.GroupId).GroupName,
                CourseName = dal.Courses.GetById(dal.Groups.GetById(c.GroupId).CourseId).CouresName,
                BranchName = dal.Branches.GetById(dal.Groups.GetById(c.GroupId).BranchId).Name,
                Hour = dal.Groups.GetById(c.GroupId).Hour,
                Created_at = c.Created_at,
                Created_by = c.Created_by,
                Date = c.Date,
                Reason = c.Reason
            }).ToList();
        }

        // **מתודה נוספת - קבלת כל הקבוצות שיכולות להתבטל ביום מסוים**
        public List<BLLGroupDetails> GetGroupsAvailableForCancellation(string dayOfWeek, DateTime date)
        {
            var groupsForDay = dal.Groups.GetGroupsByDayOfWeek(dayOfWeek);
            DateOnly dateOnly = DateOnly.FromDateTime(date);

            // קבלת הביטולים הקיימים לתאריך זה
            var existingCancellations = dal.LessonCancellations.GetCancellationsByDate(date);
            var cancelledGroupIds = existingCancellations.Select(c => c.GroupId).ToHashSet();

            return groupsForDay
                .Where(g => !cancelledGroupIds.Contains(g.GroupId)) // רק קבוצות שלא בוטלו
                .Select(g => new BLLGroupDetails
                {
                    GroupId = g.GroupId,
                    GroupName = g.GroupName,
                    DayOfWeek = g.DayOfWeek,
                    CourseName = dal.Courses.GetById(g.CourseId).CouresName,
                    BranchName = dal.Branches.GetById(g.BranchId).Name,
                    Hour = g.Hour,
                    AgeRange = g.AgeRange,
                    MaxStudents = g.MaxStudents,
                    Sector = g.Sector,
                    StartDate = g.StartDate,
                    NumOfLessons = g.NumOfLessons,
                    LessonsCompleted = g.LessonsCompleted
                }).ToList();
        }

        public void Delete(int id)
        {
            dal.LessonCancellations.Delete(id);
        }

        public List<BLLLessonCancellations> Get()
        {
            return dal.LessonCancellations.Get().Select(c => new BLLLessonCancellations()
            {
                Id = c.Id,
                GroupId = c.GroupId,
                Created_at = c.Created_at,
                Created_by = c.Created_by,
                Date = c.Date,
                Reason = c.Reason
            }).ToList();
        }

        public BLLLessonCancellations GetById(int id)
        {
            LessonCancellations lc = dal.LessonCancellations.GetById(id);
            BLLLessonCancellations lcn = new BLLLessonCancellations()
            {
                Reason = lc.Reason,
                Date = lc.Date,
                Created_at = lc.Created_at,
                Created_by = lc.Created_by,
                GroupId = lc.GroupId,
                Id = id,
            };
            return lcn;
        }

        public void Update(BLLLessonCancellations lc)
        {
            var m = dal.LessonCancellations.GetById(lc.Id);
            m.Created_at = lc.Created_at;
            m.Created_by = lc.Created_by;
            m.Date = lc.Date;
            m.Reason = lc.Reason;
            m.GroupId = lc.GroupId;
            m.Id = lc.Id;
            dal.LessonCancellations.Update(m);
        }
    }
}
