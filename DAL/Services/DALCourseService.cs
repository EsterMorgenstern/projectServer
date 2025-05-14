
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

        public void Delete(int courseId)
        {
            var course = dbcontext.Courses.SingleOrDefault(x => x.CourseId == courseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }

            dbcontext.Courses.Remove(course);
            dbcontext.SaveChanges();

        }

        public List<Course> Get()
        {
            return dbcontext.Courses.ToList();
        }

        public Course GetById(int id)
        {
            var course = dbcontext.Courses.SingleOrDefault(x => x.CourseId == id);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found.");
            }
            return course;
        }

        public void Update(Course course)
        {
            dbcontext.Courses.Update(course);
            dbcontext.SaveChanges();
        }
    }
}
