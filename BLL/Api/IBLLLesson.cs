using BLL.Models;

namespace BLL.Api
{
    public interface IBLLLesson
    {
        List<BLLLesson> Get();
        Task Create(BLLLesson lesson);
        public BLLLesson GetById(int id);
        public void Delete(int id);
        public void Update(BLLLesson lesson);
        Task GenerateLessonsForGroup(int groupId, DateOnly startDate, int numOfLessons, string dayOfWeek, TimeOnly lessonHour, int instructorId, string createdBy);
        List<BLLLesson> GetByGroupId(int groupId);
        List<(int LessonId, string GroupName, string LessonStatus,string City)> GetGroupsWithLessonsByDate(DateOnly date);

        void CancelLesson(int lessonId, string reason, string canceledBy);
        void CancelAllGroupsForDay(string dayOfWeek, DateOnly date, string reason, string createdBy);
        List<BLLLesson> GetCanceledLessonsByDate(DateOnly date);
        void UndoCancelLesson(int lessonId, string undoBy);
        List<BLLLesson> GetCanceledLessons();

        Task CreateCompletionLesson(int groupId, DateOnly completionDate, TimeOnly completionHour,
        int instructorId, string createdBy);
        void MarkLessonAsCompletion(int lessonId, string markedBy);
        List<BLLLesson> GetCompletionLessons();
        List<BLLLesson> GetCompletionLessonsByGroupId(int groupId);
        List<LessonCalendarItemDto> GetLessonsForCalendarByDate(DateOnly date);
        List<LessonCalendarItemDto> GetLessonsForCalendarByDateRange(DateOnly startDate, DateOnly endDate);
    }
}
