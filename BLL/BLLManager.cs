
using BLL.Api;
using BLL.Services;
using DAL;
using DAL.Api;

namespace BLL
{
    public class BLLManager : IBLL
    {
        public IBLLStudent Students { get; }
        public IBLLInstructor Instructors { get; }
        public IBLLCourse Courses { get; }
        public IBLLStudentCourse StudentCourses { get; }

        public BLLManager()
        {
            IDAL dal = new DALManager();
            Students = new BLLStudentService(dal);
            Instructors = new BLLInstructorService(dal);
            Courses = new BLLCourseService(dal);
            StudentCourses = new BLLStudentCourseService(dal);

        }
    }
}
