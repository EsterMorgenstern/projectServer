using DAL.Api;
using DAL.Models;


namespace DAL.Services
{
    public class DALCourseService : IDALCourse
    {
        dbcontext dbcontext;
        public DALCourseService(dbcontext data)
        {
            dbcontext = data;
        }
        public void Create(Course course)
        {
            dbcontext.Courses.Add(course);
            dbcontext.SaveChanges();
        }

        public void Delete(Course course)
        {
            dbcontext.Courses.Remove(course);
            dbcontext.SaveChanges();
        }

        public List<Course> Get()
        {
           return dbcontext.Courses.ToList();   
        }

        public Course GetById(int id)
        {
            return dbcontext.Courses.SingleOrDefault(x => x.CourseId == id);
        }
      
        public void Update(Course course)
        {
            dbcontext.Courses.Update(course);
            dbcontext.SaveChanges();
        }
    }
}
