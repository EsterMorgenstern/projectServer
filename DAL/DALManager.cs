using DAL.Api;
using DAL.Models;
using DAL.Services;

namespace DAL
{
    public class DALManager : IDAL
    {
        Dbcontext data = new Dbcontext();

        public DALManager()
        {
            Students = new DALStudentService(data);
            Instructors = new DALInstructorService(data);
            Courses = new DALCourseService(data);
            StudentCourses = new DALStudentCourseService(data);
            Groups = new DALGroupService(data);
            GroupStudents = new DALGroupStudentService(data);
            Branches = new DALBranchService(data);
            Attendances = new DALAttendanceService(data);
        }

        public IDALStudent Students { get; }
        public IDALInstructor Instructors { get; }
        public IDALCourse Courses { get; }
        public IDALStudentCourse StudentCourses { get; }
        public IDALGroup Groups { get; }
        public IDALGroupStudent GroupStudents { get; }
        public IDALBranch Branches { get; }
        public IDALAttendance Attendances { get; }

    }
}
