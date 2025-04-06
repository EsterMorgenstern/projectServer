using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALStudentCourseService : IDALStudentCourse
    {
        dbcontext dbcontext;
        public DALStudentCourseService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(StudentCourse studentCourse)
        {
            throw new NotImplementedException();
        }

        public void Delete(StudentCourse studentCourse)
        {
            throw new NotImplementedException();
        }

        public List<StudentCourse> Get()
        {
            throw new NotImplementedException();
        }

        public StudentCourse GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(StudentCourse studentCourse)
        {
            throw new NotImplementedException();
        }
    }
}
