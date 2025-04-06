namespace DAL.Api
{
    public interface IDAL
    {
        public IDALStudent Students { get; }
        public IDALInstructor Instructors { get; }
        public IDALCourse Courses { get; }
        public IDALStudentCourse StudentCourses { get; }
    }
}
