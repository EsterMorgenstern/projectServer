using DAL.Models;

namespace DAL.Api
{
    public interface IDALStudentCourse
    {
        List<StudentCourse> Get();
        void Create(StudentCourse studentCourse);
        StudentCourse GetById(int cId, int sId);
        StudentCourse GetByIdCourse(int cId);
        List<StudentCourse> GetByIdStudent(int sId);
        void Delete(StudentCourse studentCourse);
        void Update(StudentCourse studentCourse);
    }
}
