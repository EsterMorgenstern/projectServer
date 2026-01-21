using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALLessonService : IDALLesson
    {
        private readonly dbcontext dbcontext;

        public DALLessonService(dbcontext context)
        {
            dbcontext = context;
        }

        public List<Lesson> Get()
        {
            return dbcontext.Lessons.ToList();
        }

        public void Create(Lesson lesson)
        {
            dbcontext.Lessons.Add(lesson);
            dbcontext.SaveChanges();
        }

        public Lesson GetById(int id)
        {
            var lesson = dbcontext.Lessons.SingleOrDefault(x => x.LessonId == id);
            if (lesson == null)
            {
                throw new KeyNotFoundException($"Lesson with ID {id} not found.");
            }
            return lesson;
        }

        public void Delete(int id)
        {
            var lesson = dbcontext.Lessons.SingleOrDefault(x => x.LessonId == id);
            if (lesson != null)
            {
                dbcontext.Lessons.Remove(lesson);
                dbcontext.SaveChanges();
            }
        }

        public void Update(Lesson lesson)
        {
            dbcontext.Lessons.Update(lesson);
            dbcontext.SaveChanges();
        }
    }
}
