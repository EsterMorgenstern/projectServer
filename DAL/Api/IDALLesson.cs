using DAL.Models;

namespace DAL.Api
{
    public interface IDALLesson
    {
        List<Lesson> Get();
        Lesson GetById(int id);
        void Create(Lesson lesson);
        void Update(Lesson lesson);
        void Delete(int id);
    }
}
