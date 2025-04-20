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
            dbcontext.StudentCourses.Add(studentCourse);
            dbcontext.SaveChanges();
        }

        public void Delete(StudentCourse studentCourse)
        {
            dbcontext.StudentCourses.Remove(studentCourse);
            dbcontext.SaveChanges();
        }

        public List<StudentCourse> Get()
        {
            return dbcontext.StudentCourses.ToList();
        }

        public StudentCourse GetById(int cId, int sId)
        {
            return dbcontext.StudentCourses.SingleOrDefault(x => x.CourseId == cId && x.StudentId == sId)
                   ?? throw new InvalidOperationException("StudentCourse not found.");
        }

        public void Update(StudentCourse studentCourse)
        {
            dbcontext.StudentCourses.Update(studentCourse);
            dbcontext.SaveChanges();
        }
    }
}
