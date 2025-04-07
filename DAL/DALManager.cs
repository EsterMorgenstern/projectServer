using DAL.Api;
using DAL.Models;
using DAL.Services;

namespace DAL
{
    public class DALManager : IDAL
    {
        dbcontext data = new dbcontext();

        public DALManager()
        {
            Students = new DALStudentService(data);
            Instructors = new DALInstructorService(data);
            Courses = new DALCourseService(data);
            StudentCourses = new DALStudentCourseService(data);
        }

        public IDALStudent Students { get; }
        public IDALInstructor Instructors { get; }
        public IDALCourse Courses { get; }
        public IDALStudentCourse StudentCourses { get; }

    }
}
