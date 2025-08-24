using DAL.Api;
using DAL.Models;

public class DALLessonCancellationsService : IDALLessonCancellations
{
    private readonly dbcontext dbcontext;

    public DALLessonCancellationsService(dbcontext data)
    {
        dbcontext = data;
    }

    public void Create(LessonCancellations lc)
    {
        dbcontext.LessonCancellations.Add(lc);
        dbcontext.SaveChanges();
    }

    public void Delete(int id)
    {
        var trackedL = dbcontext.LessonCancellations.Find(id);
        if (trackedL != null)
        {
            dbcontext.LessonCancellations.Remove(trackedL);
            dbcontext.SaveChanges();
        }
    }

    public List<LessonCancellations> Get()
    {
        if (dbcontext.LessonCancellations == null || !dbcontext.LessonCancellations.Any())
        {
            return new List<LessonCancellations>();
        }
        return dbcontext.LessonCancellations.ToList();
    }

    public void Update(LessonCancellations lc)
    {
        dbcontext.LessonCancellations.Update(lc);
        dbcontext.SaveChanges();
    }

    public LessonCancellations GetById(int id)
    {
        var lc = dbcontext.LessonCancellations.SingleOrDefault(x => x.Id == id);
        if (lc == null)
        {
            throw new KeyNotFoundException($"lc with ID {id} not found.");
        }
        return lc;
    }

    /// <summary>
    /// ביטול שיעורים ליום מסוים
    /// </summary>
    /// <param name="dayOfWeek"></param>
    /// <param name="date"></param>
    /// <param name="reason"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdAt"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void CancelAllGroupsForDay(string dayOfWeek, DateOnly date, string reason, string createdBy, DateOnly createdAt)
    {
        try
        {
            // קבלת כל הקבוצות שמתקיימות ביום הנתון
            var groupsForDay = dbcontext.Groups
                .Where(g => g.DayOfWeek == dayOfWeek)
                .ToList();

            foreach (var group in groupsForDay)
            {
                // בדיקה אם כבר קיים ביטול לקבוצה זו בתאריך זה
                var existingCancellation = dbcontext.LessonCancellations
                    .FirstOrDefault(c => c.GroupId == group.GroupId && c.Date == date);

                if (existingCancellation == null)
                {
                    LessonCancellations cancellation = new LessonCancellations()
                    {
                        GroupId = group.GroupId,
                        Date = date,
                        Reason = reason,
                        Created_at = createdAt,
                        Created_by = createdBy
                    };

                    dbcontext.LessonCancellations.Add(cancellation);
                }
            }

            dbcontext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while canceling lessons for day {dayOfWeek}.", ex);
        }
    }

    public List<LessonCancellations> GetCancellationsByDate(DateTime date)
    {
        try
        {
            DateOnly searchDate = DateOnly.FromDateTime(date);

            return dbcontext.LessonCancellations
                .Where(c => c.Date == searchDate)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving cancellations for date {date.ToShortDateString()}.", ex);
        }
    }

    /// <summary>
    /// מחיקת כל הביטולים ליום מסוים
    /// </summary>
    /// <param name="dayOfWeek"></param>
    /// <param name="date"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void RemoveAllCancellationsForDay(string dayOfWeek, DateTime date)
    {
        try
        {
            DateOnly searchDate = DateOnly.FromDateTime(date);

            // קבלת כל הקבוצות שמתקיימות ביום הנתון
            var groupsForDay = dbcontext.Groups
                .Where(g => g.DayOfWeek == dayOfWeek)
                .Select(g => g.GroupId)
                .ToList();

            // מציאת כל הביטולים לקבוצות אלו בתאריך הנתון
            var cancellationsToRemove = dbcontext.LessonCancellations
                .Where(c => groupsForDay.Contains(c.GroupId) && c.Date == searchDate)
                .ToList();

            if (cancellationsToRemove.Any())
            {
                dbcontext.LessonCancellations.RemoveRange(cancellationsToRemove);
                dbcontext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while removing cancellations for day {dayOfWeek}.", ex);
        }
    }
}
