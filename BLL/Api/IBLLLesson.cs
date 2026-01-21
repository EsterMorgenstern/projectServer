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
    }
}
