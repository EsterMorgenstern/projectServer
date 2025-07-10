using BLL.Models;

namespace BLL.Api
{
    public interface IBLLLessonCancellations
    {
        void Create(BLLLessonCancellations lc);
        void Delete(int id);
        List<BLLLessonCancellations> Get();
        BLLLessonCancellations GetById(int id);
        void Update(BLLLessonCancellations lc);
        void CancelAllGroupsForDay(string dayOfWeek, DateTime date, string reason, string createdBy);
        List<BLLLessonCancellations> GetCancellationsByDate(DateTime date);
        void RemoveAllCancellationsForDay(string dayOfWeek, DateTime date);
        List<BLLLessonCancellationsDetails> GetCancellationDetailsByDate(DateTime date);
        List<BLLGroupDetails> GetGroupsAvailableForCancellation(string dayOfWeek, DateTime date);
    }
}
