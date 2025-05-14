using DAL.Models;

namespace DAL.Api
{
    public interface IDALCourse
    {
        List<Course> Get();
        void Create(Course course);
        Course GetById(int id);
        void Delete(int courseId);
        void Update(Course course);
    }
}
