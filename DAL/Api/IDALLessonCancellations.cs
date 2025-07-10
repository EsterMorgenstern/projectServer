using DAL.Models;

namespace DAL.Api
{
    public interface IDALLessonCancellations
    {
        List<LessonCancellations> Get();
        LessonCancellations GetById(int id);
        void Create(LessonCancellations lc);
        void Delete(int id);
        void Update(LessonCancellations lc);
        void CancelAllGroupsForDay(string dayOfWeek, DateOnly date, string reason, string createdBy, DateOnly createdAt);
        List<LessonCancellations> GetCancellationsByDate(DateTime date);
        void RemoveAllCancellationsForDay(string dayOfWeek, DateTime date);
    }
}
