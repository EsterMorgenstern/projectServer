using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudentCourse
    {
        List<StudentCourse> Get();
        void Create(StudentCourse studentCourse);
        StudentCourse GetById(int id);
        void Delete(StudentCourse studentCourse);
        void Update(StudentCourse studentCourse);
    }
}
