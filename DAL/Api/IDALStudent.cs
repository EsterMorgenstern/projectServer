using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudent
    {
        List<Student> Get();
        void Create(Student student);
        Student GetById(int id);
        void Delete(Student student);
        void Update(Student student);
    }
}
